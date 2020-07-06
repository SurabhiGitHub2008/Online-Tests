using Newtonsoft.Json;
using OnlineOrderInfo.Helper;
using OnlineOrderInfo.Models;
using RestSharp;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineOrderInfo.Service
{
    public class MenuItemRepository
    {
        private UserRestClient userRestClient;

        internal List<Menu> GetMenuItems(string nameFilter, int restaurantId)
        {
            //retieve the restaurant menus
            List<RestaurantMenu> resturntMenus = GetResturantMenus(restaurantId);

            //retrieve all menus under menugroups
            List<Menu> menus = GetListofMenus(resturntMenus);

            List<Menu> filteredMenus = new List<Menu>();

            RedisConnectorHelper redisConnector = new RedisConnectorHelper();
            IDatabase cacheDB;

            //retrieve the filtered menus
            try
            {
                using (redisConnector.Connection)
                {
                    cacheDB = redisConnector.CacheDB;
                    List<List<HashEntry>> menuHashEntries = new List<List<HashEntry>>();

                    cacheDB.StringSet("menus", JsonConvert.SerializeObject(menus));

                    foreach (var menu in menus)
                    {
                        var menuHashEntry = ConvertToHashEntryList(menu);
                        cacheDB.HashSet("menu:" + menu.MenuItemId, menuHashEntry.ToArray());

                        var hashEntries = cacheDB.HashGetAll("menu:" + menu.MenuItemId);

                        Menu filteredMenu = GetFilteredMenus(nameFilter, hashEntries);
                        if(!(filteredMenu==null))
                        { 
                        filteredMenus.Add(filteredMenu);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                redisConnector.Connection.Close();
            }
            return filteredMenus;
        }

        private Menu GetFilteredMenus(string nameFilter, HashEntry[] hashEntries)
        {
            Menu retMenu = new Menu();
            retMenu.MenuItemName = hashEntries.Where(entry => entry.Name == "MenuItemName").First().Value;

            if (retMenu.MenuItemName.ToUpper().Contains(nameFilter.ToUpper()))
            {
                retMenu.MenuItemId = (int)hashEntries.Where(entry => entry.Name == "MenuItemId").First().Value;
                retMenu.MenuName = hashEntries.Where(entry => entry.Name == "MenuName").First().Value;
                retMenu.DeliveryTypeCode = hashEntries.Where(entry => entry.Name == "DeliveryTypeCode").First().Value;
                return retMenu;
            }
            else
            {
                return null;
            }

        }

        internal List<Menu> GetListofMenus(List<RestaurantMenu> resturntMenus)
        {
            List<Menu> menus = new List<Menu>();

            try
            {
                foreach (var rMenu in resturntMenus)
                {
                    userRestClient = new UserRestClient("menuId", "menus", rMenu.Id.ToString());

                    IRestResponse response = userRestClient.Client.Execute(userRestClient.Request);
                    if (!response.IsSuccessful)
                    {
                        throw new ApplicationException(response.ErrorMessage);
                    }
                    var rootCollection = JsonConvert.DeserializeObject<MenuRootCollection>(response.Content);
                    foreach (var menuItemGroup in rootCollection.MenuItemgroups)
                    {
                        foreach (var menuItem in menuItemGroup.MenuItems)
                        {
                            menus.Add(new Menu(rootCollection.Name, rootCollection.DeliveryTypeCode, menuItem.Id, menuItem.Name));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                
            }
            return menus;
        }

        private List<RestaurantMenu> GetResturantMenus(int restaurantId)
        {
            List<RestaurantMenu> resturntMenus;
            userRestClient = new UserRestClient("restaurantId", "restaurants", restaurantId.ToString());

            try
            {
                IRestResponse response = userRestClient.Client.Execute(userRestClient.Request);
                if (!response.IsSuccessful)
                {
                    throw new ApplicationException(response.ErrorMessage);
                }

                Restaurant restaurantDetails = JsonConvert.DeserializeObject<Restaurant>(response.Content);
                resturntMenus = restaurantDetails.Menus;

            }
            catch (Exception e)
            {

                throw e;
            }

            return resturntMenus;
        }
        private List<HashEntry> ConvertToHashEntryList(object instance)
        {
            var propertiesInHashEntryList = new List<HashEntry>();
            foreach (var property in instance.GetType().GetProperties())
            {
                propertiesInHashEntryList.Add(new HashEntry(property.Name, instance.GetType().GetProperty(property.Name).GetValue(instance).ToString()));

            }
            return propertiesInHashEntryList;
        }
    }
}
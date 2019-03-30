using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickType;
using Refit;

namespace TimetableApp.API
{
    //Refit documentation for guidence
    //[Headers("Ocp-Apim-Subscription-Key: 9c3efb16d55a471781e299822b6b01be")]
    interface ITimetableAPI
    {
        /*
        //query=orange&offset=0&limit=10
        [Get("/grocery/products/?query={query}&offset={offset}&limit={limit}")]
        Task<RootObject> GetUsers(string query, int offset, int limit);

            https://jsonplaceholder.typicode.com/users

        */
        

        [Get("/api/Timetables")]
        Task<List<Welcome>> GetAllRooms();

        //api/Timetable/room#/today
        [Get("/api/Timetables/rooms/{roomid}")]
        Task<List<Welcome>> GetRoom(int roomid);

        //api/Timetable/227/Monday
        [Get("/api/Timetables/{roomid}/{day}")]
        Task<List<Welcome>> GetRoomOnDay(int roomid, string day);

        //used for testing json from jsonplaceholder.com/users
        [Get("/users")]
        Task<List<Welcome>> GetUser();

        [Get("/api/values")]
        Task<string> GetValues();



        /*
        //room#/tormorrow
        [Get("")]
        Task<Timetable> GetUsers(string query, int offset, int limit);

        //room#/thisweek
        [Get("")]
        Task<Timetable> GetUsers(string query, int offset, int limit);
        */

    }
}
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

        [Get("/api/Timetables/rooms")]
        Task<List<Welcome>> GetAllRooms();

        //api/Timetable/room#/today
        [Get("/api/Timetables/{roomid}")]
        Task<Welcome> GetRoom(int roomid);

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
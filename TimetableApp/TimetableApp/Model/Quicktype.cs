﻿namespace QuickType
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome
    {
        [JsonProperty("timetable_Id")]
        public long TimetableId { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTimeOffset EndTime { get; set; }

        [JsonProperty("room")]
        public Room Room { get; set; }

        [JsonProperty("calendar")]
        public Calendar Calendar { get; set; }

        [JsonProperty("calendar_Id")]
        public long CalendarId { get; set; }

        [JsonProperty("room_Id")]
        public long RoomId { get; set; }
    }

    public partial class Calendar
    {
        [JsonProperty("calendar_Id")]
        public long CalendarId { get; set; }

        [JsonProperty("calendarDate")]
        public DateTimeOffset CalendarDate { get; set; }

        [JsonProperty("dayofWeek")]
        public string DayofWeek { get; set; }

        [JsonProperty("weekNumber")]
        public long WeekNumber { get; set; }
    }

    public partial class Room
    {
        [JsonProperty("room_Id")]
        public long RoomId { get; set; }

        [JsonProperty("room_no")]
        public long RoomNo { get; set; }

        [JsonProperty("isBusy")]
        public bool isBusy { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
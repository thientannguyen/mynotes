using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyNotes.API.Logging
{
    /// <summary>
    /// This class is used in order to serialize information for application inside 
    /// </summary>
    public class TrackingObject
    {
        private readonly object _trackingObject;
        private Func<string, string> _eventNamePattern;
        private const BindingFlags PublicMemberFlags = BindingFlags.Public | BindingFlags.Instance;

        public TrackingObject(object trackingObject)
        {
            _trackingObject = trackingObject;
            _eventNamePattern = name => name;
        }

        public static TrackingObject Create(object obj) => new TrackingObject(obj);

        public TrackingObject WithEventNamePattern(Func<string, string> applyEventNamePattern) =>
            new TrackingObject(_trackingObject) { _eventNamePattern = applyEventNamePattern };

        public IDictionary<string, string> Properties =>
            GetMembers(_trackingObject)
                .Select(ReplacePassword)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        public string EventName =>
            _eventNamePattern(_trackingObject.GetType().Name);


        public static IEnumerable<KeyValuePair<string, string>> GetMembers<T>(T @object) =>
            @object
                .GetType()
                .GetProperties(PublicMemberFlags)
                .Where(m => m.CanRead)
                .Select(m => new { Key = m.Name, Value = m.GetValue(@object)?.ToString() })
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        public static KeyValuePair<string, string> ReplacePassword(KeyValuePair<string, string> kvp) =>
            kvp.Key.ToLower() == "password"
                ? new KeyValuePair<string, string>(kvp.Key, "****")
                : kvp;
    }
}

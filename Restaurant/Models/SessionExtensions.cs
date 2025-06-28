using System.Text.Json;

namespace Restaurant.Models
{
    public static class SessionExtensions
    {
        //save object to session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        //get object from session
        public static T Get<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}

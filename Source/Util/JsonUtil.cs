using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

using System.Collections.Generic;
using System.Net;
namespace RimTalk.Util;

public static class JsonUtil
{
    public static string SerializeToJson<T>(T obj)
    {
        // Create a memory stream for serialization
        using var stream = new MemoryStream();
        // Create a DataContractJsonSerializer
        var serializer = new DataContractJsonSerializer(typeof(T));

        // Serialize the ApiRequest object
        serializer.WriteObject(stream, obj);

        // Convert the memory stream to a string
        return Encoding.UTF8.GetString(stream.ToArray());
    }
    
    public static Dictionary<string, object> DeserializeToDictionary(string json)
    {
        var result = new Dictionary<string, object>();
    
    // 移除空白字符
    json = json.Trim();
    
    if (!json.StartsWith("{") || !json.EndsWith("}"))
        throw new FormatException("Invalid JSON object");
    
    // 提取内部内容
    string inner = json.Substring(1, json.Length - 2).Trim();
    
    var parts = SplitJsonObject(inner);
    foreach (var part in parts)
    {
        var keyValue = part.Split(new[] { ':' }, 2);
        if (keyValue.Length != 2) continue;
        
        string key = keyValue[0].Trim().Trim('"');
        string value = keyValue[1].Trim();
        
        result[key] = ParseJsonValue(value);
    }
    
    return result;
    }
    
    private static object ParseJsonValue(string value)
    {
        if (value.StartsWith("{") && value.EndsWith("}"))
        {
            return DeserializeToDictionary(value);
        }
        else if (value.StartsWith("[") && value.EndsWith("]"))
        {
            return ParseJsonArray(value);
        }
        else if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return value.Substring(1, value.Length - 2);
        }
        else if (value == "true" || value == "false")
        {
            return bool.Parse(value);
        }
        else if (double.TryParse(value, out double number))
        {
            return number;
        }
        
        return value;
    }

    private static List<object> ParseJsonArray(string arrayJson)
    {
        var result = new List<object>();
        
        string inner = arrayJson.Substring(1, arrayJson.Length - 2).Trim();
        if (string.IsNullOrEmpty(inner)) return result;
        
        var items = SplitJsonArray(inner);
        foreach (var item in items)
        {
            result.Add(ParseJsonValue(item.Trim()));
        }
        
        return result;
    }
    
    private static List<string> SplitJsonObject(string inner)
    {
        var result = new List<string>();
        int depth = 0;
        int start = 0;
        
        for (int i = 0; i < inner.Length; i++)
        {
            char c = inner[i];
            
            if (c == '{' || c == '[') depth++;
            else if (c == '}' || c == ']') depth--;
            else if (c == ','&&(inner[i-1]=='\"'||inner[i-1]==']'||inner[i-1]=='}')&&(inner[i+1]=='\"'||inner[i+1]=='{')&& depth == 0)
            {
                result.Add(inner.Substring(start, i - start));
                start = i + 1;
            }
        }
        
        if (start < inner.Length)
            result.Add(inner.Substring(start));
        
        return result;
    }

    private static List<string> SplitJsonArray(string inner)
    {
        return SplitJsonObject(inner); // 使用相同的逻辑
    }

    public static T DeserializeFromJson<T>(string json)
    {
        string sanitizedJson = Sanitize(json, typeof(T));
        
        try
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sanitizedJson));
            // Create an instance of DataContractJsonSerializer
            var serializer = new DataContractJsonSerializer(typeof(T));

            // Deserialize the JSON data
            return (T)serializer.ReadObject(stream);
        }
        catch (Exception ex)
        {
            Logger.Error($"Json deserialization failed for {typeof(T).Name}\n{json}");
            throw;
        }
    }


    /// <summary>
    /// The definitive sanitizer that fixes structural, syntax, and formatting errors from LLM-generated JSON.
    /// </summary>
    /// <param name="text">The raw string from the LLM.</param>
    /// <param name="targetType">The C# type we are trying to deserialize into.</param>
    /// <returns>A cleaned and likely valid JSON string.</returns>
    public static string Sanitize(string text, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        string sanitized = text.Replace("```json", "").Replace("```", "").Trim();

        int startIndex = sanitized.IndexOfAny(['{', '[']);
        int endIndex = sanitized.LastIndexOfAny(['}', ']']);

        if (startIndex >= 0 && endIndex > startIndex)
        {
            sanitized = sanitized.Substring(startIndex, endIndex - startIndex + 1).Trim();
        }
        else
        {
            return string.Empty;
        }

        if (sanitized.Contains("]["))
        {
             sanitized = sanitized.Replace("][", ",");
        }
        if (sanitized.Contains("}{"))
        {
            sanitized = sanitized.Replace("}{", "},{");
        }
        
        if (sanitized.StartsWith("{") && sanitized.EndsWith("}"))
        {
            string innerContent = sanitized.Substring(1, sanitized.Length - 2).Trim();
            if (innerContent.StartsWith("[") && innerContent.EndsWith("]"))
            {
                sanitized = innerContent;
            }
        }

        bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(targetType) && targetType != typeof(string);
        if (isEnumerable && sanitized.StartsWith("{"))
        {
            sanitized = $"[{sanitized}]";
        }

        return sanitized;
    }
}
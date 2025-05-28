using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

public static class DataLogger
{
    public static string LogList<T>(List<T> list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            sb.AppendLine($"[{i}] {LogObject(list[i])}");
        }
        return sb.ToString();
    }

    public static string LogDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        StringBuilder sb = new StringBuilder();
        int idx = 0;
        foreach (var kvp in dict)
        {
            sb.AppendLine($"[{idx}] Key: {LogObject(kvp.Key)}, Value: {LogObject(kvp.Value)}");
            idx++;
        }
        return sb.ToString();
    }

    public static string LogObject(object obj, int depth = 0)
    {
        if (obj == null)
            return "null";

        Type type = obj.GetType();

        // 기본형/문자열
        if (type.IsPrimitive || obj is string || obj is decimal)
            return obj.ToString();

        // enum 처리
        if (type.IsEnum)
            return Enum.GetName(type, obj);

        // Dictionary 처리
        if (obj is IDictionary dict)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var key in dict.Keys)
            {
                sb.Append($"Key: {LogObject(key, depth + 1)}, Value: {LogObject(dict[key], depth + 1)}; ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        // IEnumerable (List, 배열 등)
        if (obj is IEnumerable enumerable && !(obj is string))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in enumerable)
            {
                sb.Append(LogObject(item, depth + 1));
                sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }

        // 구조체/클래스의 필드 출력
        StringBuilder log = new StringBuilder();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            // enum 내부 value__ 필드 무시
            if (field.Name == "value__") continue;

            object value = field.GetValue(obj);
            log.Append($"{field.Name}: {LogObject(value, depth + 1)}, ");
        }
        return log.ToString();
    }
}
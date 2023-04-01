using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rce_File.Inner_C_Script.EventCenter
{
    public class EventCenter :MonoBehaviour
    {
        private static readonly 
            IDictionary<MyEventType, UnityEvent> 
            Events = new Dictionary<MyEventType, UnityEvent>();

        public static void Subscribe
            (MyEventType eventType, UnityAction listener) //对应事件类型添加listener
        {
            
            UnityEvent thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent)) //有这个事件类型
            {
                thisEvent.AddListener(listener);
            }
            else //没有这个事件类型，添加该事件类型
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventType, thisEvent);
            }
        }

        public static void Unsubscribe
            (MyEventType type, UnityAction listener) //删除某一事件的listener
        {
            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(MyEventType type) //找到对应事件类型，
        {
            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent)) 
            {
                thisEvent.Invoke();//广播事件发生
            }
        }
    }
}



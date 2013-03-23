using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace XLVExploreTracker
{
    public abstract class TrackController
    {
        public abstract void Track(string arg);
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
        public static void Track(string trackerName,string arg)
        {
            getTracker(trackerName).Track(arg);
        }
        public static string GetDescription(string trackerName, string arg)
        {
            var key=trackerName+','+arg;
            if(!trackerDescriptionCache.Value.ContainsKey(key)){
                trackerDescriptionCache.Value.Add(key,(getTracker(trackerName) as TrackerImplementation).GetDescription(arg));
            }
            return trackerDescriptionCache.Value[key];
        }
        static Lazy<Dictionary<string, string>> trackerDescriptionCache = new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        static TrackController getTracker(string trackerName)
        {
            return Type.GetType("XLVExploreTracker.Trackers." + trackerName)
                .GetConstructor(System.Type.EmptyTypes)
                .Invoke(new Object[0]) as TrackController;
        }
        protected void Fire(string format, params object[] args)
        {
            Event(this, format, args);
        }
        public static event TrackControllerEventHandler Event;
        public static TrackingMode Mode=TrackingMode.TrackPages;
        public delegate void TrackControllerEventHandler(TrackController sender, string format, params object[] args);
        public enum TrackingMode{
            RefreshUrlList,
            JustClear,
            TrackPages
        }
    }
}

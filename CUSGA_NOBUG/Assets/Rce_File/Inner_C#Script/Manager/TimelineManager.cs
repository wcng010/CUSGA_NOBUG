using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TimelineManager :BaseManager<TimelineManager>
{ 
   public PlayableDirector bridgeTimeline; 
   public PlayableDirector doorTimeline;
   public PlayableDirector bucketTimeline;
   public PlayableDirector fishingRodTimeline;
   public PlayableDirector leafTimeline;
   public PlayableDirector TreeTimeline;
   private void Start()
   {
      bucketTimeline.Play();
      fishingRodTimeline.Play();
      leafTimeline.Play();
   }
}

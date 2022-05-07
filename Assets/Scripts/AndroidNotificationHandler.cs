using System;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID

    private const string ChannelID = "notificaiton_channel";

    public void ScheduleNotification(DateTime dateTime)
    {
        var notificationChannel = new AndroidNotificationChannel
                                  {
                                      Id          = ChannelID,
                                      Name        = "Notification Channel",
                                      Description = "Random Description",
                                      Importance  = Importance.Default
                                  };

        var notification = new AndroidNotification
                           {
                               Title     = "Energy Recharged!",
                               Text      = "Your Energy has Recharged, come back to play again!",
                               SmallIcon = "default",
                               LargeIcon = "default",
                               FireTime  = dateTime
                           };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);
        AndroidNotificationCenter.SendNotification(notification, ChannelID);
    }
#endif
}
using Android.Content;
using Android.Media;
using Android.OS;

namespace NSUtils
{
    public enum EnumTypeOfSound
    {
        Message,
        Event,
        Call,
        Other
    }

    public static class ExtensionMethodsMedia
    {
        public static void PlaySound(this Context context, EnumTypeOfSound typeOfSound)
        {            
            var ringtoneUri = Android.Net.Uri.Parse("");

            if (typeOfSound == EnumTypeOfSound.Message)
            {
                ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);                
            }
            else if (typeOfSound == EnumTypeOfSound.Event)
            {
                ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);                
            }
            else if (typeOfSound == EnumTypeOfSound.Call)
            {
                ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);                
            }
            else
            {
                ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
            }

            PlaySound(context, ringtoneUri);
        }

        private static void PlaySound(this Context context, Android.Net.Uri ringtoneUri)
        {            
            var mediaPlayer = MediaPlayer.Create(context, ringtoneUri);
            
            if (mediaPlayer != null)
            {
                mediaPlayer.SetVolume(100, 100);

                mediaPlayer.SetAudioStreamType(Stream.Music);
                mediaPlayer.Start();
                
                new Handler().PostDelayed(() =>
                {
                    mediaPlayer.Stop();
                }, 5000);
            }
        }
    }
}
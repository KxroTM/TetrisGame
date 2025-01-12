using NAudio.Wave;

namespace TetrisGame
{
    public static class SoundManager
    {
        private static WaveOutEvent soundEffect;
        private static AudioFileReader audioFile;

        public static void PlaySound(string filePath, float volume = 0.5f)
        {
            if (soundEffect != null)
            {
                soundEffect.Stop();
                soundEffect.Dispose();
            }

            audioFile = new AudioFileReader(filePath);
            soundEffect = new WaveOutEvent();
            soundEffect.Init(audioFile);
            soundEffect.Volume = volume;
            soundEffect.Play();
        }

        public static void SetVolume(float volume)
        {
            if (soundEffect != null)
            {
                soundEffect.Volume = volume;
            }
        }
    }
}

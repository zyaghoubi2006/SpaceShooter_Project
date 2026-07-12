using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class AudioManager : IDisposable
    {
        private WaveOutEvent bgmOutput;
        private AudioFileReader bgmReader;
        private LoopStream bgmLoop;

        private float musicVolume = 0.5f;
        private float sfxVolume = 1.0f;
        private bool musicMuted = false;
        private bool sfxMuted = false;

        private List<WaveOutEvent> activeSfxOutputs = new List<WaveOutEvent>();
        private List<AudioFileReader> activeSfxReaders = new List<AudioFileReader>();

        public void PlayBackgroundMusic(string path)
        {
            StopBackgroundMusic();

            if (!File.Exists(path))
                throw new FileNotFoundException("Music file not found", path);

            bgmReader = new AudioFileReader(path);
            bgmReader.Volume = musicMuted ? 0f : musicVolume;


            bgmLoop = new LoopStream(bgmReader);

            bgmOutput = new WaveOutEvent();
            bgmOutput.Init(bgmLoop);
            bgmOutput.Play();
        }


        public void StopBackgroundMusic()
        {
            bgmOutput?.Stop();
            bgmOutput?.Dispose();
            bgmOutput = null;

            bgmLoop?.Dispose();
            bgmLoop = null;

            bgmReader?.Dispose();
            bgmReader = null;
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Math.Max(0f, Math.Min(1f, volume));

            if (bgmReader != null && !musicMuted)
                bgmReader.Volume = musicVolume;
        }

        public void SetSfxVolume(float volume)
        {
            sfxVolume = Math.Max(0f, Math.Min(1f, volume));
        }

        public void SetMusicMuted(bool muted)
        {
            musicMuted = muted;
            if (bgmReader != null)
                bgmReader.Volume = muted ? 0f : musicVolume;
        }

        public void SetSfxMuted(bool muted)
        {
            sfxMuted = muted;
        }

        public void SwitchTrack(string fullPath)
        {
            PlayBackgroundMusic(fullPath);
        }


        public void PlaySoundEffect(string relativePath)
        {
            if (sfxMuted) return;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("SFX file not found", fullPath);

            var reader = new AudioFileReader(fullPath);
            reader.Volume = sfxVolume;

            var output = new WaveOutEvent();
            output.Init(reader);

            output.PlaybackStopped += (s, e) =>
            {
                output.Dispose();
                reader.Dispose();

                activeSfxOutputs.Remove(output);
                activeSfxReaders.Remove(reader);
            };

            activeSfxOutputs.Add(output);
            activeSfxReaders.Add(reader);

            output.Play();
        }

        public void Dispose()
        {
            StopBackgroundMusic();

            foreach (var output in activeSfxOutputs)
            {
                output.Stop();
                output.Dispose();
            }

            foreach (var reader in activeSfxReaders)
            {
                reader.Dispose();
            }

            activeSfxOutputs.Clear();
            activeSfxReaders.Clear();
        }
    }
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => sourceStream.Length;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (bytesRead == 0)
                {
                    sourceStream.Position = 0;
                }
                else
                {
                    totalBytesRead += bytesRead;
                }
            }

            return totalBytesRead;
        }
    }
}

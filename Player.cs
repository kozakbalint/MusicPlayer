using System;
using NAudio.Wave;

namespace MusicPlayer
{
    class Player
    {
        private DirectSoundOut output;
        private AudioFileReader reader;
        private bool repeat;

        public PlaybackState GetPlaybackState => output.PlaybackState;
        public bool GetRepeate => repeat;
        public bool SetRepeate { set => repeat = value; }

        public Player(string path, double volume)
        {
            reader = new AudioFileReader(path) { Volume = (float)volume };
            output = new DirectSoundOut(200);
            output.PlaybackStopped += Output_PlaybackStopped;
            var wc = new WaveChannel32(reader);
            wc.PadWithZeroes = false;
            output.Init(wc);
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (repeat)
            {
                if(output != null)
                {
                    SetPosition(0);
                    output.Play();
                }
            }
            else
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    output.Stop();
                }
                output.Dispose();
                output = null;
            }

            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        public void Play(PlaybackState currentState, double currentVolume)
        {
            if ((currentState == PlaybackState.Stopped || currentState == PlaybackState.Paused) && output != null)
            {
                output.Play();
                reader.Volume = (float)currentVolume;
            }    
        }

        public void Stop()
        {
            if (output != null)
            {
                output.Stop();
            }
        }

        public void Pause()
        {
            if (output != null)
            {
                output.Pause();
            }
        }

        public void PlayPause(double currenVolume)
        {
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    Pause();
                }
                else
                {
                    Play(output.PlaybackState, currenVolume);
                }
            }
            else
            {
                Play(PlaybackState.Stopped, currenVolume);
            }
        }

        public double GetLenght()
        {
            if (reader != null)
            {
                return reader.TotalTime.TotalSeconds;
            }
            else
            {
                return 0;
            }
        }

        public double GetPosition()
        {
            if (reader != null)
            {
                return reader.CurrentTime.TotalSeconds;
            }
            else
            {
                return 0;
            }
        }

        public void SetPosition(double value)
        {
            if (reader != null)
            {
                reader.CurrentTime = TimeSpan.FromSeconds(value);
            }
        }

        public void SetVolume(float value)
        {
            if (reader != null)
            {
                reader.Volume = value;
            }
        }
    }
}

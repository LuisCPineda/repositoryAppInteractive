using NAudio.Wave;
using NAudio.Wave.SampleProviders;

// Auteur: Éric Wenaas

namespace Model
{
    

    public class Lecteur
    {
        private WaveOutEvent? _outputDevice;
        private AudioFileReader? _audioFile;

        public bool IsPaused => _outputDevice?.PlaybackState == PlaybackState.Paused;
        public bool IsPlaying => _outputDevice?.PlaybackState == PlaybackState.Playing;
        public bool IsActive => IsPlaying || IsPaused;
        public int Volume
        {
            get
            {
                if (_outputDevice == null)
                {
                    return 0;
                }
                else
                {
                    return (int)(_outputDevice.Volume * 100);
                }
            }
            set
            {
                if (_outputDevice != null)
                {
                    _outputDevice.Volume = Math.Clamp(value, 0, 100) / 100.0f;
                }
                
            }
        }

        public void Play(string path)
        {
            if (IsPlaying)
            {
                _outputDevice?.Stop();
                _audioFile?.Dispose();
            }
            _outputDevice = new WaveOutEvent();
            _audioFile = new AudioFileReader(path);
            _outputDevice.Init(_audioFile);
            _outputDevice.Play();
        }

        public void Pause()
        {
            _outputDevice?.Pause();
        }

        public void UnPause()
        {
            _outputDevice?.Play();
        }

        public void Stop()
        {
            _outputDevice?.Stop();
            _audioFile?.Dispose();
        }

        public string GetRemainingTime()
        {
            try
            {
                TimeSpan remaining = _audioFile?.TotalTime - _audioFile?.CurrentTime ?? TimeSpan.Zero;
                if (remaining.Milliseconds > 0 && remaining < _audioFile?.TotalTime)
                {
                    remaining = remaining.Add(new TimeSpan(0, 0, 1));
                }

                return remaining.ToString(@"mm\:ss");
            }
            catch (NullReferenceException ex)
            {
                return "00:00";
            }
        }
    }
} 
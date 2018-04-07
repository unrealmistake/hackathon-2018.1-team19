using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.IO;
using System.Threading;

namespace Roughness {
    class SoundUnit {
        public AudioBuffer audioBuffer;
        public WaveFormat waveFormat;
        public uint[] packetsInfo;
        public int PlayTimeS;
        public float Volume;
        public SoundUnit(AudioBuffer audio_buffer, WaveFormat wave_format, uint[] packets_info, int play_time_s, float volume) {
            audioBuffer = audio_buffer;
            waveFormat = wave_format;
            packetsInfo = packets_info;
            PlayTimeS = play_time_s;
            Volume = volume;
        }
    }

    class AudioEngine {
        public static Dictionary<string, SoundUnit> Sounds = new Dictionary<string, SoundUnit>();
        private static XAudio2 xaudio2 = new XAudio2();

        public static void AudioEngineInit () {
            MasteringVoice masteringVoice = new MasteringVoice(xaudio2);
            DownloadSoundFile("explosion", @"..\..\GameRes\exp.wav", 1);
            DownloadSoundFile("theme_1", @"..\..\GameRes\Cliff_Side.wav",147);
            DownloadSoundFile("pickup", @"..\..\GameRes\pickup.wav", 1, (float)0.3); 
        }

        private static void DownloadSoundFile(string sound_name, string fileName,int play_time_s, float volume = 1) {
            SoundStream stream = new SoundStream(File.OpenRead(fileName));
            WaveFormat waveFormat = stream.Format;
            AudioBuffer buffer = new AudioBuffer {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };
            stream.Close();
            Sounds.Add(sound_name, new SoundUnit(buffer, waveFormat, stream.DecodedPacketsInfo, play_time_s, volume));
        }
        public void PlaySound(object file_name) {
            string FileName = (string)file_name;
            lock (this) { // Подразумеваетсья лок Sounds[FileName]
                SourceVoice sourceVoice = new SourceVoice(xaudio2, Sounds[FileName].waveFormat, true);
                sourceVoice.SubmitSourceBuffer(Sounds[FileName].audioBuffer, Sounds[FileName].packetsInfo);
                sourceVoice.SetVolume(Sounds[FileName].Volume);
                sourceVoice.Start();
                Thread.Sleep(Sounds[FileName].PlayTimeS * 1000);
                sourceVoice.DestroyVoice();
                sourceVoice.Dispose();
            }

        }
    }
}

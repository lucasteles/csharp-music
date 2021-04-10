using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Seconds = System.Single;
using Samples = System.Single;
using Pulse = System.Single;
using Hz = System.Single;

var sampleRate = 48000f;
var volume = .5f;

void Play(float[] wave)
{
    var filename = "./output.bin";
    var bytes = wave.SelectMany(BitConverter.GetBytes).ToArray();
    if (File.Exists(filename)) File.Delete(filename);
    File.WriteAllBytes(filename, bytes);
    Process.Start($"ffplay", $"-showmode 1 -f f32le -ar {sampleRate} ./output.bin");
}

Pulse[] GetWave(float step, Seconds duration) =>
    Enumerable
        .Range(0, (int) (sampleRate * duration))
        .Select(x => x * step)
        .Select(x => (float) Math.Sin(x))
        .Select(x => x * volume)
        .ToArray();

Pulse[] Freq(Hz hz, Seconds duration)
{
    var step = (float) (hz * 2 * Math.PI) / sampleRate;
    return GetWave(step, duration);
}

var duration = 2f;
var wave = new[]
    {
        Freq(440f, .5f),
        Freq(540f, .5f),
    }
    .SelectMany(x => x).ToArray();

Play(wave);
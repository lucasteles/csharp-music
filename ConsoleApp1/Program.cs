using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Seconds = System.Single;
using Samples = System.Single;
using Pulse = System.Single;
using Hz = System.Single;

var sampleRate = 48000f;
var pitchStandard = 440f;
var volume = .5f;

void Play(float[][] wave)
{
    var filename = "./output.bin";
    var bytes = wave.SelectMany(x => x).SelectMany(BitConverter.GetBytes).ToArray();
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

var duration = .3f;
var wave =
    Enumerable.Range(0, 10)
        .Select(i => Freq(pitchStandard + i * 100f, duration))
        .ToArray();

Play(wave);
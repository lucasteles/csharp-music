using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Seconds = System.Single;
using Samples = System.Single;
using Pulse = System.Single;
using Hz = System.Single;
using Semitons = System.Single;

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
        .Select(MathF.Sin)
        .Select(x => x * volume)
        .ToArray();

Pulse[] Freq(Hz hz, Seconds duration)
{
    var step = hz * 2 * MathF.PI / sampleRate;
    var output = GetWave(step, duration);

    var attack =
        Enumerable.Range(0, output.Length)
            .Select(x => MathF.Min(1, x / 1000f));
    var release = attack.Reverse();
    var wave = output
        .Zip(attack, (w, v) => w * v)
        .Zip(release, (w, v) => w * v)
        .ToArray();

    return wave;
}

Hz F(Semitons n) => (float) (pitchStandard * Math.Pow(Math.Pow(2, 1.0 / 12.0), n));
Pulse[] Note(Semitons n, Seconds duration) => Freq(F(n), duration);
var duration = .5f;
var wave = new[]
{
    Note(0, duration),
    Note(0, duration),
    Note(0, duration),
    Note(0, duration),
    Note(0, duration),
    Note(0, duration),
};
Play(wave);
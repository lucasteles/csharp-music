using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

var sampleRate = 48000f;
var volume = .5f;

void Play(float[] wave)
{
    var filename = "./output.bin";
    var bytes = wave.SelectMany(BitConverter.GetBytes).ToArray();
    if (File.Exists(filename)) File.Delete(filename);
    File.WriteAllBytes(filename,bytes);
    Process.Start($"ffplay",$"-showmode 1 -f f32le -ar {sampleRate} ./output.bin");
}

float[] GetWave(float step, float duration) =>
    Enumerable
        .Range(0, (int)(sampleRate * duration))
        .Select(x => x * step)
        .Select(x => (float)Math.Sin(x))
        .Select(x => x * volume)
        .ToArray();

var step =(float)(440f * 2 * Math.PI) / sampleRate;
var duration = 2f;
Play(GetWave(step, duration));

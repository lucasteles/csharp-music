using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

void Play(IEnumerable<float> wave)
{
    var filename = "./output.bin";
    var bytes = wave.SelectMany(BitConverter.GetBytes).ToArray();
    if (File.Exists(filename)) File.Delete(filename);
    File.WriteAllBytes(filename,bytes);
    Process.Start("ffplay"," -f f32le -ar 48000 ./output.bin");
}

var volume = .5f;
var step = .05f;
var wave = Enumerable
    .Range(0, 48000)
    .Select(x => x * step)
    .Select(x => (float)Math.Sin(x))
    .Select(x => x * volume);

Play(wave);

using System.Text;

namespace AbfAuto.CycleDetection;

public readonly struct BinnedEvents
{
    public readonly int[] Counts;
    public readonly double[] TimesMinutes;
    public readonly double[] Freqs;
    public readonly double[] FreqMinutes;
    public readonly double[] MeanAmplitude;
    public readonly Cycle[] AllCycles;
    public readonly List<Cycle>[] BinnedCycles;

    public BinnedEvents(Cycle[] cycles, double recordingLength, double binSize)
    {
        AllCycles = cycles;

        int binCount = (int)Math.Ceiling(recordingLength / binSize);
        TimesMinutes = Enumerable.Range(0, binCount).Select(x => x * binSize / 60).ToArray();

        BinnedCycles = Enumerable.Range(0, binCount).Select(x => new List<Cycle>()).ToArray();

        foreach (Cycle cycle in cycles)
        {
            int binIndex = (int)(cycle.StartTime / binSize);
            BinnedCycles[binIndex].Add(cycle);
        }

        Counts = BinnedCycles.Select(x => x.Count).ToArray();
        Freqs = BinnedCycles.Select(x => GetFreq(x, binSize)).ToArray();
        FreqMinutes = Freqs.Select(x => x * 60).ToArray();
        MeanAmplitude = BinnedCycles
            .Select(x => x.Select(x => x.Amplitude))
            .Select(x => x.Any() ? x.Average() : 0)
            .ToArray();
    }

    private static double GetFreq(List<Cycle> cycles, double binSize)
    {
        if (cycles.Count == 0)
            return 0;

        if (cycles.Count == 1)
            return 1.0 / binSize;

        // events per second
        return (cycles.Count - 1) / (cycles.Last().StartTime - cycles.First().StartTime);
    }

    public string ToCsv(int baselineCount = 5)
    {
        StringBuilder sb = new();

        sb.AppendLine("Time (min), Events/min, Amplitude (raw), Amplitude (%)");

        double baseline = MeanAmplitude.Take(baselineCount).Average();
        double[] norm = MeanAmplitude.Select(x => x / baseline * 100).ToArray();

        for (int i = 0; i < TimesMinutes.Length; i++)
        {
            sb.AppendLine($"{TimesMinutes[i]},{FreqMinutes[i]},{MeanAmplitude[i]},{norm[i]}");
        }

        return sb.ToString();
    }
}

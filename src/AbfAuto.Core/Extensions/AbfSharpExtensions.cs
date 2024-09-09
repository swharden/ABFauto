﻿namespace AbfAuto.Core.Extensions;

public static class AbfSharpExtensions
{
    public static Sweep GetAllData(this AbfSharp.ABF abf, int channelIndex = 0)
    {
        int samplesPerSweep = abf.Header.AbfFileHeader.lNumSamplesPerEpisode / abf.Header.AbfFileHeader.nADCNumChannels;
        int sweepCount = abf.Header.AbfFileHeader.lActualEpisodes;
        double[] values = new double[samplesPerSweep * sweepCount];

        int offset = 0;
        for (int sweepIndex = 0; sweepIndex < abf.SweepCount; sweepIndex++)
        {
            float[] sweepValues = abf.GetSweep(sweepIndex, channelIndex);
            for (int i = 0; i < sweepValues.Length; i++)
            {
                values[offset++] = sweepValues[i];
            }
        }

        return new Sweep(values, abf.SampleRate, 0, channelIndex, 0);
    }
}

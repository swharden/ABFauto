﻿using System.Diagnostics;

namespace AbfAuto.Core;

public static class AbfSharpExtensions
{
    public static Trace GetAllData(this AbfSharp.ABF abf, int channelIndex = 0)
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

        return new Trace(values, 1.0 / abf.Header.SampleRate);
    }
}

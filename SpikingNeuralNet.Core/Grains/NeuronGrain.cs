using SpikingNeuralNet.Core.GrainModel;

namespace SpikingNeuralNet.Core.Grains;

public sealed class NeuronGrain : Grain, INeuronGrain
{
    private const double Threshold = 1.0;
    private const double TauMs = 20.0;      // membrane time constant
    private const double DelayMs = 1.0;     // synaptic delay

    private double _potential;
    private double _lastTimeMs;

    // incoming synapses
    private readonly Dictionary<long, double> _weights = new();
    
    // outgoing connections
    private long[] _targets = [];
    
    public Task ReceiveSpike(Spike spike) {
        // leak lazily, decaying by the simulated elapsed time
        _potential *= Math.Exp(
            -(spike.TimeMs - _lastTimeMs) / TauMs
        );

        _lastTimeMs = spike.TimeMs;

        _potential += _weights.GetValueOrDefault(spike.Source);

        if(_potential >= Threshold) {
            _potential = 0;

            var output = new Spike {
                Source = this.GetPrimaryKeyLong(),
                TimeMs = spike.TimeMs + DelayMs
            };

            foreach(var target in _targets) {
                _ = GrainFactory.GetGrain<INeuronGrain>(target).ReceiveSpike(output);
            }
        }

        return Task.CompletedTask;
    }
}
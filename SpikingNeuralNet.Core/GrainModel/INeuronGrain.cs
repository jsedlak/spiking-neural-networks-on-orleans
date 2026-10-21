using Orleans.Concurrency;

namespace SpikingNeuralNet.Core.GrainModel;

[Alias("INeuronGrain")]
public interface INeuronGrain : IGrainWithIntegerKey
{
    [OneWay]
    Task ReceiveSpike(Spike spike);
}
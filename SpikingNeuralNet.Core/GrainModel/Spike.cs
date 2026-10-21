namespace SpikingNeuralNet.Core.GrainModel;

[GenerateSerializer, Immutable]
public sealed class Spike
{
    [Id(0)]
    public long Source { get; set; }
    
    [Id(1)]
    public double TimeMs{ get; set; }
}
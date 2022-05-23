using ZeissMachineStreamCore.Entities;

namespace ZeissMachineStreamCore.Interfaces
{
    public interface ISaveData
    {
        Task StoreEventAsync(StreamEvent streamEvent, string source);
    }
}

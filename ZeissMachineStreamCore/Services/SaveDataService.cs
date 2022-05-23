using ZeissMachineStreamCore.Entities;
using ZeissMachineStreamCore.Interfaces;

namespace ZeissMachineStreamCore.Services
{
    public class SaveDataService : ISaveData
    {
        private readonly IMongo mongo;

        public SaveDataService(IMongo mongo)
        {
            this.mongo = mongo;
        }

        public async Task StoreEventAsync(StreamEvent streamEvent, string source)
        {
            await mongo.InsertEventAsync(new WebSocketStream(streamEvent, source));
        }
    }
}

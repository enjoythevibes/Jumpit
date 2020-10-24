using System.Collections.Generic;
using System.Linq;
using enjoythevibes.Serialization;

namespace enjoythevibes.Data
{
    public class PlayerData
    {
        public int recordScore;
        public int crystals;
        
        public float sensitivity;
        public bool audioEnabled;
        public int resolutionHeight;

        public int currentGradientSchemeID;
        public List<int> gradientSchemesBought = new List<int>();

        private const float sensitivityDefault = 5f;
        private const bool audioEnabledDefault = true;
        private const int resolutionHeightDefault = 700;
        private const int currentGradientSchemeIDDefault = 1;

        public void SetEmpty()
        {
            recordScore = 0;
            crystals = 0;
            currentGradientSchemeID = currentGradientSchemeIDDefault;
            gradientSchemesBought.Add(currentGradientSchemeIDDefault);
            SetDefault();
        }

        public void SetDefault()
        {
            sensitivity = sensitivityDefault;
            audioEnabled = audioEnabledDefault;
            resolutionHeight = resolutionHeightDefault;
        }

        public byte[] GetBytesData()
        {
            var types = new int[]
            {
                sizeof(int), // recordScore
                sizeof(int), // crystals
                sizeof(float), // sensitivity
                sizeof(bool), // audioEnabled
                sizeof(int), // resolutionHeight
                sizeof(int), // currentGradientSchemeID
                sizeof(int), // gradientSchemesBought.Count
                sizeof(int) * gradientSchemesBought.Count // gradientSchemesBought
            };
            var size = types.Sum();
            var bytes = new byte[size];

            var offset = 0;
            DataConverter.SetBytes(bytes, recordScore, ref offset);
            DataConverter.SetBytes(bytes, crystals, ref offset);
            DataConverter.SetBytes(bytes, sensitivity, ref offset);
            DataConverter.SetBytes(bytes, audioEnabled, ref offset);
            DataConverter.SetBytes(bytes, resolutionHeight, ref offset);
            DataConverter.SetBytes(bytes, currentGradientSchemeID, ref offset);
            DataConverter.SetBytes(bytes, gradientSchemesBought, ref offset);
            return bytes;
        }

        public void ReadFromBytes(byte[] bytes)
        {
            var offset = 0;
            var recordScoreData = DataConverter.ReadInt32Bytes(bytes, ref offset);
            if (recordScoreData.HasValue)
                recordScore = recordScoreData.Value;
            else
                recordScore = 0;
            
            var crystalsData = DataConverter.ReadInt32Bytes(bytes, ref offset);
            if (crystalsData.HasValue)
                crystals = crystalsData.Value;
            else
                crystals = 0;
            
            var sensitivityData = DataConverter.ReadFloatBytes(bytes, ref offset);
            if (sensitivityData.HasValue)
                sensitivity = sensitivityData.Value;
            else
                sensitivity = sensitivityDefault;

            var audioEnabledData = DataConverter.ReadBoolBytes(bytes, ref offset);
            if (audioEnabledData.HasValue)
                audioEnabled = audioEnabledData.Value;
            else
                audioEnabled = audioEnabledDefault;

            var resolutionHeightData = DataConverter.ReadInt32Bytes(bytes, ref offset);
            if (resolutionHeightData.HasValue)
                resolutionHeight = resolutionHeightData.Value;
            else
                resolutionHeight = resolutionHeightDefault;

            var currentGradientSchemeIDData = DataConverter.ReadInt32Bytes(bytes, ref offset);
            if (currentGradientSchemeIDData.HasValue)
                currentGradientSchemeID = currentGradientSchemeIDData.Value;
            else
                currentGradientSchemeID = currentGradientSchemeIDDefault;

            var gradientSchemesBoughtCount = DataConverter.ReadInt32Bytes(bytes, ref offset);
            if (gradientSchemesBoughtCount.HasValue)
            {
                DataConverter.ReadListInt(bytes, ref offset, gradientSchemesBoughtCount.Value, gradientSchemesBought);
            }
        }
    }
}
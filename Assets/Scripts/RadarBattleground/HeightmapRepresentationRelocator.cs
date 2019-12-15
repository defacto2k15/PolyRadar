using UnityEngine;

namespace Assets.Scripts.RadarBattleground
{
    public class HeightmapRepresentationRelocator
    {
        private ComputeShader _transportComputeShader;

        public HeightmapRepresentationRelocator(ComputeShader transportComputeShader)
        {
            _transportComputeShader = transportComputeShader;
        }

        public HeightmapArray TextureToArray(RenderTexture heightTexture)
        {
            var pixelsCount = heightTexture.width * heightTexture.height;
            var outBuffer = new ComputeBuffer(pixelsCount,4,ComputeBufferType.Default);

            var cs = _transportComputeShader;
            var kernelIdx = cs.FindKernel("CSTextureToBuffer");
            cs.SetTexture(kernelIdx, "_InputTexture",heightTexture);
            cs.SetBuffer(kernelIdx, "_OutputBuffer", outBuffer);

            DispatchTransferKernel(cs, kernelIdx, new Vector2Int(heightTexture.width, heightTexture.height));

            var outArray = new float[pixelsCount];
            outBuffer.GetData(outArray);
            outBuffer.Release();

            return new HeightmapArray(outArray, new Vector2Int(heightTexture.width, heightTexture.height));
        }

        public RenderTexture ArrayToTexture(HeightmapArray heightmap)
        {
            var pixelsCount = heightmap.Size.x * heightmap.Size.y;
            var inBuffer = new ComputeBuffer(pixelsCount,4,ComputeBufferType.Default);
            inBuffer.SetData(heightmap.Array);

            var outTexture = new RenderTexture(heightmap.Size.x, heightmap.Size.y, 0, RenderTextureFormat.RFloat);
            outTexture.enableRandomWrite = true;
            outTexture.Create();

            var cs = _transportComputeShader;
            var kernelIdx = cs.FindKernel("CSBufferToTexture");
            cs.SetTexture(kernelIdx, "_OutputTexture",outTexture);
            cs.SetBuffer(kernelIdx, "_InputBuffer", inBuffer);

            DispatchTransferKernel(cs, kernelIdx, heightmap.Size);

            var outArray = new float[pixelsCount];
            inBuffer.Release();

            return outTexture;
        }

        private static void DispatchTransferKernel(ComputeShader cs, int kernelIdx, Vector2Int size)
        {
            var pixelsCount = size.x * size.y;
            cs.SetInt("g_InputTextureWidth", size.x);
            cs.SetInt("g_InputTextureHeight", size.y);

            uint xSize;
            uint ySize;
            uint zSize;
            cs.GetKernelThreadGroupSizes(kernelIdx, out xSize, out ySize, out zSize);

            var callCount = Mathf.CeilToInt(pixelsCount / (float) xSize);
            cs.Dispatch(kernelIdx, callCount, 1, 1);
        }
    }
}
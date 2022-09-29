using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.DataStructures
{
    public struct VertexInfo : IVertexType
    {
        /// <summary> 世界坐标 </summary>
        public Vector2 position;
        /// <summary> 像素颜色 </summary>
        public Color color;
        /// <summary> 纹理坐标+透明度？ </summary>
        public Vector3 texCoord;
        
        private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
        {
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
        });
        
        public VertexInfo(Vector2 position, Color color, Vector3 texCoord)
        {
            this.position = position;
            this.color = color;
            this.texCoord = texCoord;
        }

        public VertexDeclaration VertexDeclaration => VertexInfo._vertexDeclaration;
    }
}
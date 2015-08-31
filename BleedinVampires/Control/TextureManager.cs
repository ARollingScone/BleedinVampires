using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace BleedinVampires.Control
{
    struct TextureObj
    {
        int TextureId;
        Texture Texture;
    }

    class TextureManager
    {
        public Dictionary<int, Texture> LoadedTextures;

        public TextureManager()
        {
            LoadedTextures = new Dictionary<int, Texture>();

            Texture test = new Texture("C:\\Users\\James\\Documents\\Visual Studio 2013\\Projects\\BleedinVampires\\BleedinVampires\\Assets\\Textures\\Test\\testtex.jpg");

            LoadedTextures.Add(1, test);
        }
    }
}

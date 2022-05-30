using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Mesh
{
    public class Mesh
    {
    //    public int VERTEX_POS_OFFSET = 0;
    //    public int VERTEX_TEX_OFFSET = VERTEX_POS_OFFSET + sizeof(Vector3);
    //    public int VERTEX_NORMAL_OFFSET(VERTEX_TEX_OFFSET + sizeof(Vector2))
    //    public int VERTEX_TANGENT_OFFSET(VERTEX_NORMAL_OFFSET + sizeof(Vector3))

    //    struct Vertex
    //    {
    //        public Vector3 mPos;
    //        public Vector2 mTex;
    //        public Vector3 mNormal;
    //        public Vector3 mTangent;

    //        Vertex(Vector3 pos, Vector2 tex, Vector3 normal, Vector3 tangent)
    //        {
    //            mPos = pos;
    //            mTex = tex;
    //            mNormal = normal;
    //            mTangent = tangent;
    //        }
    //    };


    //    bool loadMesh(string filename)
    //    {
    //        clear();

    //        Assimp::Importer importer;

    //        const aiScene* scene = importer.ReadFile(
    //        filename.c_str(),
    //        aiProcess_Triangulate | aiProcess_GenSmoothNormals | aiProcess_FlipUVs |
    //            aiProcess_CalcTangentSpace);

    //        if (scene) {
    //            return initFromScene(scene, filename);
    //        }
    //        return false;
    //    }

    //    void render();

    //        private:
    //    bool initFromScene(const aiScene* scene, const std::string& filename);
    //    void initMesh(unsigned int index, const aiMesh* mesh);
    //        bool initMaterials(const aiScene* scene, const std::string& filename);
    //    void clear();


    //        struct MeshEntry
    //        {
    //            MeshEntry();

    //            ~MeshEntry();

    //            bool init(
    //                    const std::vector<Vertex>& vertices,
    //                const std::vector<unsigned int>& indices
    //            );

    //            GLuint VA;
    //            GLuint VB;
    //            GLuint IB;
    //            unsigned int numIndices;
    //            unsigned int materialIndex;
    //        };

    //        std::vector<MeshEntry> mEntries;
    //        std::vector<std::shared_ptr<Texture>> mTextures;
    //    };
    }
}

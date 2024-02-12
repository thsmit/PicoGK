//
// SPDX-License-Identifier: CC0-1.0
//
// This example code file is released to the public under Creative Commons CC0.
// See https://creativecommons.org/publicdomain/zero/1.0/legalcode
//
// To the extent possible under law, LEAP 71 has waived all copyright and
// related or neighboring rights to this PicoGK Example Code.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//


using PicoGK;
using System.Numerics;


namespace Leap71
{
    using ShapeKernel;

    namespace ShapeKernelExamples
    {
        class LogoShowCase
        {
            /// <summary>
            /// This example shows how an imported TGA image can be "embossed" onto a base plate object
            /// ans how this plate can be transformed into more complex shapes.
            /// </summary>
            public static void Task()
            {
                try
                {
                    //Step 1: Load Logo as TGA image
                    string strFilename = "MyLogo.tga";
                    TgaIo.LoadTga(strFilename, out Image oImage);

                    {
                        //Step 1: Base Plate with a Logo
                        LocalFrame oLocalFrame  = new LocalFrame();
                        float fPlateThickness   = 2f;
                        float fRefWidth         = 50;
                        BaseLogoBox oShape      = new BaseLogoBox(oLocalFrame, fPlateThickness, fRefWidth, oImage, fGetEmbossHeight);
                        Voxels oVoxels          = oShape.voxConstruct();
                        Sh.PreviewVoxels(oVoxels, Cp.clrRandom());
                    }

                    //{
                    //    //Step 2: Add Transformation
                    //    LocalFrame oLocalFrame  = new LocalFrame();
                    //    float fPlateThickness   = 2f;
                    //    float fRefWidth         = 50;
                    //    BaseLogoBox oShape      = new BaseLogoBox(oLocalFrame, fPlateThickness, fRefWidth, oImage, fGetEmbossHeight);
                    //    oShape.SetTransformation(vecGetTrafo);
                    //    Voxels oVoxels          = oShape.voxConstruct();
                    //    Sh.PreviewVoxels(oVoxels, Cp.clrRandom());
                    //}
                }
                catch (Exception e)
                {
                    Library.Log($"Failed run example: \n{e.Message}"); ;
                }
            }

            /// <summary>
            /// This function inputs a grayscale value (e.g. of a pixel in the image) and correlates it to an "emboss" height.
            /// The grayscale value is normalised between 0 and 1 (representing black and white).
            /// </summary>
            public static float fGetEmbossHeight(float fGrayValue)
            {
                //Option 1:
                //float fEmbossHeight1 = 3 * fGrayValue;
                //return fEmbossHeight1;

                //Option 2: Invert
                float fEmbossHeight2 = 3 - 3 * fGrayValue;
                return fEmbossHeight2;
            }

            /// <summary>
            /// This function applies a coordinate transformation to a base shape object.
            /// In this case it transforms a box in carthesian coordinates to a cylindrical shape.
            /// This trnasformation is applied point-wise to each mesh vertex that makes up the base shape.
            /// </summary>
            public static Vector3 vecGetTrafo(Vector3 vecPt)
            {
                //Option 1: Map onto basic cylinder
                float fX            = vecPt.X;
                float fY            = vecPt.Y;
                float fZ            = vecPt.Z;
                float fNewZ         = fY;
                float fNewRadius    = 30f + fZ;
                float fNewPhi       = -0.04f * fX;
                return VecOperations.vecGetCylPoint(fNewRadius, fNewPhi, fNewZ);

                ////Option 2: More complex cylinder mapping with overhang adaptation
                //float fAngle        = 30f;
                //float fX            = vecPt.X;
                //float fY            = vecPt.Y;
                //float fZ            = vecPt.Z;
                //float fNewPhi       = -0.04f * fX;
                //float fNewZ         = fY + 5f * MathF.Cos(3f * fNewPhi) + MathF.Tan(fAngle / 180f * MathF.PI) * fZ;
                //float fNewRadius    = 30f + 5f * MathF.Sin(3f * fNewPhi) + fZ;
                //return VecOperations.vecGetCylPoint(fNewRadius, fNewPhi, fNewZ);
            }
        }
    }
}
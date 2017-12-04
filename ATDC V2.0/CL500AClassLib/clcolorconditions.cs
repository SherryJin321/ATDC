using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_500ATest
{
    class clcolorconditions
    {
        /// <summary>
        /// The enumeration of observer 
        /// </summary>
        public enum CL_OBSERVER
        {
            CL_OBS_02DEG = 0,       //!< 2 degree
            CL_OBS_10DEG,           //!< 10 degree
            CL_OBS_NUM,

            CL_OBS_FIRST = CL_OBS_02DEG,
            CL_OBS_LAST = CL_OBS_10DEG,
            CL_OBS_DEFAULT = CL_OBS_02DEG,
        }                
        
        /// <summary>
        /// The enumeration of color space
        /// </summary>
        public enum CL_COLORSPACE
        {
            CL_COLORSPACE_EVXY = 0,             //!< Ev, x, y
            CL_COLORSPACE_EVUV,                 //!< Ev, u乫, v乫
            CL_COLORSPACE_EVTCPJISDUV,          //!< Ev, Correlated color temperature Tcp(JIS), delta uv
            CL_COLORSPACE_EVTCPDUV,             //!< Ev, Correlated color temperature Tcp, delta uv
            CL_COLORSPACE_EVDWPE,               //!< Ev, Dominant wavelength, Excitation purity
            CL_COLORSPACE_XYZ,                  //!< X, Y, Z
            CL_COLORSPACE_RENDERING,            //!< Color Rendering Index (Ra, R1 to R15)
            CL_COLORSPACE_PW,                   //!< Peak wavelength
            CL_COLORSPACE_SPC,                  //!< Illuminance spectral data
            CL_COLORSPACE_SCOTOPIC,             //!< Ev, Ev', S/P ratio
            CL_COLORSPACE_NUM,

            CL_COLORSPACE_FIRST = CL_COLORSPACE_EVXY,
            CL_COLORSPACE_LAST = CL_COLORSPACE_SCOTOPIC,
            CL_COLORSPACE_DEFAULT = CL_COLORSPACE_EVXY
        }        
    }
}

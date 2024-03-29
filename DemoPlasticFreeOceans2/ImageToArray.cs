﻿using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using NumSharp;
using System.Runtime.InteropServices;
using System;
using System.Drawing.Imaging;

namespace DemoPlasticFreeOceans2
{
    public class ImageToArray
    {

        public string filname = "test.jpg";
        public string model_version = "pdetect";
        public int port = 8501;
        public string host_ip = "localhost";
        public string host_ip_slow = "52.142.123.3";
        public string host_ip_fast = "52.156.195.130";
        public double thresshold = .5;

        private static readonly HttpClient client = new HttpClient(); 

        public async Task<double[]> request_AIAsync(MemoryStream a)
        {
            
                 Image h = System.Drawing.Image.FromStream(a);
            Bitmap bmp = new Bitmap(h);
            
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            // Get the address of the first line.
            int[,,] result = new int[bmp.Width, bmp.Height, 3];

            for (int column = 0; column < bmp.Height; column++)
            {
                for (int row = 0; row < bmp.Width; row++)
                {
                    var color = bmp.GetPixel(row, column);
                    var red = color.R;
                    var blue = color.B;
                    var green = color.G;

                    result[row, column, 0] = red;
                    result[row, column, 1] = green;
                    result[row, column, 2] = blue;

                }
            }

            //create new json document
            var json = JsonConvert.SerializeObject(result);
            var str = "{\"instances\": [" + json + "]}";
            var content = new StringContent(str, Encoding.UTF8, "application/json");          
            var req = "http://" + host_ip_fast + ":" +port+ "/v1/models/" + model_version + ":predict";
            var response = await client.PostAsync(req, content);
            var answer = await response.Content.ReadAsStringAsync();
            var split = filterResponse(answer);

            return split;
        }
        public double[] filterResponse(string res)
        {
            string[] filtered = res.Split("detection");
            double[] scores = { 0.962822, 0.911204, 0.302411, 0.26634, 0.0814097, 0.0749767, 0.0269839, 0.0241598, 0.0226009 };
            /*
             * {
    "predictions": [
        {
            "detection_scores": [0.962822, 0.911204, 0.302411, 0.26634, 0.0814097, 0.0749767, 0.0269839, 0.0241598, 0.0226009, 0.0223186, 0.0211502, 0.0185568, 0.0171946, 0.0153813, 0.011343, 0.00839549, 0.00760058, 0.00747689, 0.00733272, 0.00565982, 0.00523345, 0.00508835, 0.00480356, 0.00360803, 0.00354527, 0.00330425, 0.00304319, 0.00300099, 0.00287453, 0.00273776, 0.00252415, 0.00251137, 0.00250889, 0.00243726, 0.00234017, 0.00232301, 0.00230565, 0.00208993, 0.00203004, 0.00195928, 0.0019193, 0.00187944, 0.00185684, 0.00181709, 0.00174791, 0.00168051, 0.00167894, 0.00165354, 0.00158301, 0.0015326, 0.00135631, 0.00134217, 0.00128917, 0.00126634, 0.00124575, 0.00121195, 0.0011605, 0.00115028, 0.00102981, 0.00101912, 0.000968629, 0.000935823, 0.000932152, 0.000918966, 0.000898274, 0.00087181, 0.000848016, 0.000832669, 0.000754765, 0.000742757, 0.000718497, 0.000695084, 0.000693515, 0.000679297, 0.000675375, 0.000662529, 0.000648526, 0.000645019, 0.000630288, 0.000611763, 0.000585508, 0.00056479, 0.000558787, 0.000547402, 0.00054289, 0.000536476, 0.000487988, 0.000487701, 0.000480437, 0.000479148, 0.000477892, 0.000471116, 0.00046201, 0.000451629, 0.000443307, 0.000399374, 0.000398758, 0.000395923, 0.000371631, 0.00035124, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0],
            "detection_classes": [1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0],
            "num_detections": 100.0,
            "detection_boxes": [[0.54964, 0.87844, 0.577266, 0.936501], [0.173029, 0.295429, 0.208058, 0.38303], [0.188841, 0.264037, 0.204428, 0.299708], [0.474894, 0.418843, 0.490066, 0.44902], [0.646257, 0.710859, 0.66413, 0.751497], [0.74305, 0.68607, 0.765953, 0.739282], [0.694472, 0.949354, 0.717147, 0.980115], [0.9282, 0.320001, 1.0, 0.356762], [0.735536, 0.675146, 0.771533, 0.752124], [0.473325, 0.415582, 0.488194, 0.440007], [0.0, 0.800338, 0.0488025, 0.860543], [0.191441, 0.26711, 0.208092, 0.297545], [0.905067, 0.648797, 0.917013, 0.672396], [0.696683, 0.955685, 0.709965, 0.973942], [0.691357, 0.941092, 0.722512, 0.984024], [0.330673, 0.423466, 0.342124, 0.444258], [0.362968, 0.902186, 0.422939, 0.979888], [0.299443, 0.63629, 0.30964, 0.6524], [0.00287049, 0.810471, 0.0359182, 0.853499], [0.371141, 0.913139, 0.414612, 0.972256], [0.331611, 0.429303, 0.340994, 0.446454], [0.824555, 0.419712, 0.845119, 0.450755], [0.689366, 0.941575, 0.712306, 0.976167], [0.71491, 0.358938, 0.734485, 0.387796], [0.0895157, 0.556702, 0.115711, 0.603867], [0.740195, 0.284712, 0.759624, 0.308626], [0.0718019, 0.310183, 0.142917, 0.369508], [0.680164, 0.919848, 0.712256, 0.952247], [0.951222, 0.332299, 1.0, 0.357658], [0.48814, 0.439079, 0.509595, 0.47192], [0.737491, 0.282853, 0.756245, 0.30318], [0.737483, 0.288482, 0.753035, 0.307468], [0.643332, 0.709918, 0.659787, 0.748188], [0.732967, 0.282654, 0.808843, 0.325743], [0.879579, 0.86105, 0.908809, 0.888484], [0.685198, 0.915949, 0.718778, 0.95717], [0.733293, 0.681634, 0.754217, 0.727777], [0.18069, 0.304761, 0.21497, 0.371523], [0.0170515, 0.811071, 0.0411128, 0.858503], [0.750779, 0.436718, 0.758927, 0.449746], [0.192276, 0.261687, 0.206253, 0.290058], [0.628476, 0.897514, 0.690928, 0.95596], [0.638702, 0.911458, 0.678277, 0.950159], [0.868179, 0.8457, 0.918966, 0.897267], [0.763532, 0.434089, 0.771846, 0.449283], [0.661799, 0.179785, 0.672999, 0.197977], [0.479214, 0.395637, 0.493496, 0.41599], [0.87192, 0.325799, 0.907539, 0.351918], [0.678335, 0.685431, 0.688502, 0.703435], [0.933538, 0.952975, 0.964125, 0.993555], [0.046933, 0.810165, 0.106581, 0.878221], [0.682077, 0.669869, 0.695255, 0.702982], [0.75346, 0.430199, 0.762793, 0.444909], [0.631464, 0.914619, 0.668288, 0.949701], [0.727155, 0.286551, 0.775245, 0.321042], [0.484662, 0.433947, 0.499346, 0.459837], [0.51002, 0.948774, 0.532619, 0.976917], [0.91184, 0.317893, 0.969328, 0.348203], [0.475728, 0.396489, 0.489839, 0.41634], [0.1765, 0.50877, 0.186031, 0.529093], [0.887583, 0.324266, 0.925115, 0.347378], [0.172275, 0.320944, 0.186711, 0.363188], [0.145837, 0.849332, 0.226863, 0.94199], [0.548109, 0.893497, 0.578487, 0.960267], [0.517427, 0.940035, 0.532938, 0.957419], [0.765563, 0.424545, 0.77597, 0.441235], [0.21683, 0.517817, 0.22734, 0.539643], [0.702365, 0.946027, 0.737504, 0.985714], [0.903042, 0.357678, 0.923886, 0.37824], [0.880735, 0.474967, 0.894053, 0.497373], [0.761086, 0.42467, 0.77275, 0.442625], [0.032851, 0.804082, 0.0972239, 0.873125], [0.40174, 0.920828, 0.449983, 0.977674], [0.18795, 0.286582, 0.206603, 0.33509], [0.0606664, 0.802314, 0.105066, 0.857007], [0.674144, 0.858565, 0.688464, 0.877237], [0.868514, 0.330096, 0.895718, 0.353547], [0.675127, 0.289876, 0.745081, 0.327491], [0.81851, 0.304955, 0.856821, 0.33178], [0.716884, 0.344945, 0.733699, 0.371185], [0.37494, 0.935505, 0.412056, 0.982093], [0.642751, 0.929966, 0.670514, 0.9686], [0.191855, 0.29781, 0.209989, 0.33044], [0.883812, 0.851376, 0.91018, 0.87625], [0.410569, 0.914826, 0.468664, 0.980305], [0.909697, 0.411928, 0.927485, 0.435884], [0.192459, 0.288018, 0.213965, 0.342404], [0.493548, 0.438713, 0.509313, 0.465254], [0.663551, 0.907285, 0.702806, 0.952001], [0.138699, 0.819291, 0.158212, 0.846068], [0.799223, 0.302225, 0.837693, 0.32947], [0.550957, 0.929166, 0.581618, 0.968578], [0.521508, 0.927918, 0.542212, 0.954399], [0.324484, 0.870386, 0.346761, 0.901653], [0.746644, 0.431091, 0.756913, 0.452496], [0.547747, 0.889556, 0.56968, 0.939732], [0.64144, 0.169618, 0.653519, 0.188876], [0.625836, 0.896278, 0.675178, 0.938946], [0.626567, 0.299156, 0.669499, 0.33098], [0.505915, 0.956902, 0.536304, 0.990597], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0], [0.0, 0.0, 0.0, 0.0]]
        }
    ]
}
            */

            return scores;
        }
            
    }
}

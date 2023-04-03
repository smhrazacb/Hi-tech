using System.Diagnostics;

namespace Catalog.API.Utilities
{
    public class PictureUtil
    {
        public static System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
        public static void RemoveMeta(string imagePath, string destPath) 
        {
            try
            {
                //Create Output Folder if required
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                //Getting all files from Destination Folder.
                var files = Directory.EnumerateFiles(imagePath);
                Console.WriteLine("Files: {0}", files.Count<string>().ToString());
                Console.WriteLine("Processing...");
                var stopWatch = Stopwatch.StartNew();

                foreach (var item in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item);

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(item))
                    {
                        //All Properties.
                        foreach (var propId in image.PropertyIdList)
                        {
                            if (
                    //exif properties
                    propId == 0x0001
                 || propId == 0x0002
                 || propId == 0x000b
                 || propId == 0x00fe
                 || propId == 0x00ff
                 || propId == 0x0100
                 || propId == 0x0101
                 || propId == 0x0102
                 || propId == 0x0103
                 || propId == 0x0106
                 || propId == 0x0107
                 || propId == 0x0108
                 || propId == 0x0109
                 || propId == 0x010a
                 || propId == 0x010d
                 || propId == 0x010e
                 || propId == 0x010f
                 || propId == 0x0110
                 || propId == 0x0111
                 || propId == 0x0115
                 || propId == 0x0116
                 || propId == 0x0117
                 || propId == 0x0118
                 || propId == 0x0119
                 || propId == 0x011a
                 || propId == 0x011b
                 || propId == 0x011c
                 || propId == 0x011d
                 || propId == 0x011e
                 || propId == 0x011f
                 || propId == 0x0120
                 || propId == 0x0121
                 || propId == 0x0122
                 || propId == 0x0123
                 || propId == 0x0124
                 || propId == 0x0125
                 || propId == 0x0128
                 || propId == 0x0129
                 || propId == 0x012c
                 || propId == 0x012d
                 || propId == 0x0131
                 || propId == 0x0132
                 || propId == 0x013b
                 || propId == 0x013c
                 || propId == 0x013d
                 || propId == 0x013e
                 || propId == 0x013f
                 || propId == 0x0140
                 || propId == 0x0141
                 || propId == 0x0142
                 || propId == 0x0143
                 || propId == 0x0144
                 || propId == 0x0145
                 || propId == 0x0146
                 || propId == 0x0147
                 || propId == 0x0148
                 || propId == 0x014a
                 || propId == 0x014c
                 || propId == 0x014d
                 || propId == 0x014e
                 || propId == 0x0150
                 || propId == 0x0151
                 || propId == 0x0152
                 || propId == 0x0153
                 || propId == 0x0154
                 || propId == 0x0155
                 || propId == 0x0156
                 || propId == 0x0157
                 || propId == 0x0158
                 || propId == 0x0159
                 || propId == 0x015a
                 || propId == 0x015b
                 || propId == 0x015f
                 || propId == 0x0190
                 || propId == 0x0191
                 || propId == 0x0192
                 || propId == 0x0193
                 || propId == 0x0194
                 || propId == 0x0195
                 || propId == 0x01b1
                 || propId == 0x01b2
                 || propId == 0x01b3
                 || propId == 0x01b5
                 || propId == 0x0200
                 || propId == 0x0201
                 || propId == 0x0202
                 || propId == 0x0203
                 || propId == 0x0205
                 || propId == 0x0206
                 || propId == 0x0207
                 || propId == 0x0208
                 || propId == 0x0209
                 || propId == 0x0211
                 || propId == 0x0212
                 || propId == 0x0213
                 || propId == 0x0214
                 || propId == 0x022f
                 || propId == 0x02bc
                 || propId == 0x03e7
                 || propId == 0x1000
                 || propId == 0x1001
                 || propId == 0x1002
                 || propId == 0x4746
                 || propId == 0x4747
                 || propId == 0x4748
                 || propId == 0x4749
                 || propId == 0x7000
                 || propId == 0x7010
                 || propId == 0x7031
                 || propId == 0x7032
                 || propId == 0x7034
                 || propId == 0x7035
                 || propId == 0x7036
                 || propId == 0x7037
                 || propId == 0x74c7
                 || propId == 0x74c8
                 || propId == 0x800d
                 || propId == 0x80a3
                 || propId == 0x80a4
                 || propId == 0x80a5
                 || propId == 0x80a6
                 || propId == 0x80b9
                 || propId == 0x80ba
                 || propId == 0x80bb
                 || propId == 0x80bc
                 || propId == 0x80e3
                 || propId == 0x80e4
                 || propId == 0x80e5
                 || propId == 0x80e6
                 || propId == 0x8214
                 || propId == 0x8215
                 || propId == 0x8216
                 || propId == 0x8217
                 || propId == 0x8218
                 || propId == 0x8219
                 || propId == 0x821a
                 || propId == 0x827d
                 || propId == 0x828d
                 || propId == 0x828e
                 || propId == 0x828f
                 || propId == 0x8290
                 || propId == 0x8298
                 || propId == 0x829a
                 || propId == 0x829d
                 || propId == 0x82a5
                 || propId == 0x82a6
                 || propId == 0x82a7
                 || propId == 0x82a8
                 || propId == 0x82a9
                 || propId == 0x82aa
                 || propId == 0x82ab
                 || propId == 0x82ac
                 || propId == 0x830e
                 || propId == 0x8335
                 || propId == 0x8336
                 || propId == 0x835c
                 || propId == 0x835d
                 || propId == 0x835e
                 || propId == 0x835f
                 || propId == 0x83bb
                 || propId == 0x847e
                 || propId == 0x847f
                 || propId == 0x8480
                 || propId == 0x8481
                 || propId == 0x8482
                 || propId == 0x84e0
                 || propId == 0x84e1
                 || propId == 0x84e2
                 || propId == 0x84e3
                 || propId == 0x84e4
                 || propId == 0x84e5
                 || propId == 0x84e6
                 || propId == 0x84e7
                 || propId == 0x84e8
                 || propId == 0x84e9
                 || propId == 0x84ea
                 || propId == 0x84eb
                 || propId == 0x84ec
                 || propId == 0x84ed
                 || propId == 0x84ee
                 || propId == 0x84ef
                 || propId == 0x84f0
                 || propId == 0x8546
                 || propId == 0x8568
                 || propId == 0x85b8
                 || propId == 0x85d7
                 || propId == 0x85d8
                 || propId == 0x8602
                 || propId == 0x8606
                 || propId == 0x8649
                 || propId == 0x8769
                 || propId == 0x8773
                 || propId == 0x877f
                 || propId == 0x8780
                 || propId == 0x8781
                 || propId == 0x8782
                 || propId == 0x87ac
                 || propId == 0x87af
                 || propId == 0x87b0
                 || propId == 0x87b1
                 || propId == 0x87be
                 || propId == 0x8822
                 || propId == 0x8824
                 || propId == 0x8825
                 || propId == 0x8827
                 || propId == 0x8828
                 || propId == 0x8829
                 || propId == 0x882a
                 || propId == 0x882b
                 || propId == 0x8830
                 || propId == 0x8831
                 || propId == 0x8832
                 || propId == 0x8833
                 || propId == 0x8834
                 || propId == 0x8835
                 || propId == 0x885c
                 || propId == 0x885d
                 || propId == 0x885e
                 || propId == 0x8871
                 || propId == 0x888a
                 || propId == 0x9000
                 || propId == 0x9003
                 || propId == 0x9004
                 || propId == 0x9009
                 || propId == 0x9010
                 || propId == 0x9011
                 || propId == 0x9012
                 || propId == 0x9101
                 || propId == 0x9102
                 || propId == 0x9201
                 || propId == 0x9202
                 || propId == 0x9203
                 || propId == 0x9204
                 || propId == 0x9205
                 || propId == 0x9206
                 || propId == 0x9207
                 || propId == 0x9208
                 || propId == 0x9209
                 || propId == 0x920a
                 || propId == 0x920b
                 || propId == 0x920c
                 || propId == 0x920d
                 || propId == 0x920e
                 || propId == 0x920f
                 || propId == 0x9210
                 || propId == 0x9211
                 || propId == 0x9212
                 || propId == 0x9213
                 || propId == 0x9214
                 || propId == 0x9215
                 || propId == 0x9216
                 || propId == 0x9217
                 || propId == 0x923a
                 || propId == 0x923b
                 || propId == 0x923c
                 || propId == 0x923f
                 || propId == 0x927c
                 || propId == 0x9286
                 || propId == 0x9290
                 || propId == 0x9291
                 || propId == 0x9292
                 || propId == 0x932f
                 || propId == 0x9330
                 || propId == 0x9331
                 || propId == 0x935c
                 || propId == 0x9400
                 || propId == 0x9401
                 || propId == 0x9402
                 || propId == 0x9403
                 || propId == 0x9404
                 || propId == 0x9405
                 || propId == 0x9c9b
                 || propId == 0x9c9c
                 || propId == 0x9c9d
                 || propId == 0x9c9e
                 || propId == 0x9c9f
                 || propId == 0xa000
                 || propId == 0xa001
                 || propId == 0xa002
                 || propId == 0xa003
                 || propId == 0xa004
                 || propId == 0xa005
                 || propId == 0xa010
                 || propId == 0xa011
                 || propId == 0xa101
                 || propId == 0xa102
                 || propId == 0xa20b
                 || propId == 0xa20c
                 || propId == 0xa20d
                 || propId == 0xa20e
                 || propId == 0xa20f
                 || propId == 0xa210
                 || propId == 0xa211
                 || propId == 0xa212
                 || propId == 0xa213
                 || propId == 0xa214
                 || propId == 0xa215
                 || propId == 0xa216
                 || propId == 0xa217
                 || propId == 0xa300
                 || propId == 0xa301
                 || propId == 0xa302
                 || propId == 0xa401
                 || propId == 0xa402
                 || propId == 0xa403
                 || propId == 0xa404
                 || propId == 0xa405
                 || propId == 0xa406
                 || propId == 0xa407
                 || propId == 0xa408
                 || propId == 0xa409
                 || propId == 0xa40a
                 || propId == 0xa40b
                 || propId == 0xa40c
                 || propId == 0xa420
                 || propId == 0xa430
                 || propId == 0xa431
                 || propId == 0xa432
                 || propId == 0xa433
                 || propId == 0xa434
                 || propId == 0xa435
                 || propId == 0xa460
                 || propId == 0xa461
                 || propId == 0xa462
                 || propId == 0xa480
                 || propId == 0xa481
                 || propId == 0xa500
                 || propId == 0xafc0
                 || propId == 0xafc1
                 || propId == 0xafc2
                 || propId == 0xafc3
                 || propId == 0xafc4
                 || propId == 0xafc5
                 || propId == 0xb4c3
                 || propId == 0xbc01
                 || propId == 0xbc02
                 || propId == 0xbc03
                 || propId == 0xbc04
                 || propId == 0xbc80
                 || propId == 0xbc81
                 || propId == 0xbc82
                 || propId == 0xbc83
                 || propId == 0xbcc0
                 || propId == 0xbcc1
                 || propId == 0xbcc2
                 || propId == 0xbcc3
                 || propId == 0xbcc4
                 || propId == 0xbcc5
                 || propId == 0xc427
                 || propId == 0xc428
                 || propId == 0xc429
                 || propId == 0xc42a
                 || propId == 0xc44f
                 || propId == 0xc4a5
                 || propId == 0xc51b
                 || propId == 0xc573
                 || propId == 0xc580
                 || propId == 0xc5e0
                 || propId == 0xc612
                 || propId == 0xc613
                 || propId == 0xc614
                 || propId == 0xc615
                 || propId == 0xc616
                 || propId == 0xc617
                 || propId == 0xc618
                 || propId == 0xc619
                 || propId == 0xc61a
                 || propId == 0xc61b
                 || propId == 0xc61c
                 || propId == 0xc61d
                 || propId == 0xc61e
                 || propId == 0xc61f
                 || propId == 0xc620
                 || propId == 0xc621
                 || propId == 0xc622
                 || propId == 0xc623
                 || propId == 0xc624
                 || propId == 0xc625
                 || propId == 0xc626
                 || propId == 0xc627
                 || propId == 0xc628
                 || propId == 0xc629
                 || propId == 0xc62a
                 || propId == 0xc62b
                 || propId == 0xc62c
                 || propId == 0xc62d
                 || propId == 0xc62e
                 || propId == 0xc62f
                 || propId == 0xc630
                 || propId == 0xc631
                 || propId == 0xc632
                 || propId == 0xc633
                 || propId == 0xc634
                 || propId == 0xc635
                 || propId == 0xc640
                 || propId == 0xc65a
                 || propId == 0xc65b
                 || propId == 0xc65c
                 || propId == 0xc65d
                 || propId == 0xc660
                 || propId == 0xc68b
                 || propId == 0xc68c
                 || propId == 0xc68d
                 || propId == 0xc68e
                 || propId == 0xc68f
                 || propId == 0xc690
                 || propId == 0xc691
                 || propId == 0xc692
                 || propId == 0xc6bf
                 || propId == 0xc6c5
                 || propId == 0xc6d2
                 || propId == 0xc6d3
                 || propId == 0xc6f3
                 || propId == 0xc6f4
                 || propId == 0xc6f5
                 || propId == 0xc6f6
                 || propId == 0xc6f7
                 || propId == 0xc6f8
                 || propId == 0xc6f9
                 || propId == 0xc6fa
                 || propId == 0xc6fb
                 || propId == 0xc6fc
                 || propId == 0xc6fd
                 || propId == 0xc6fe
                 || propId == 0xc714
                 || propId == 0xc715
                 || propId == 0xc716
                 || propId == 0xc717
                 || propId == 0xc718
                 || propId == 0xc719
                 || propId == 0xc71a
                 || propId == 0xc71b
                 || propId == 0xc71c
                 || propId == 0xc71d
                 || propId == 0xc71e
                 || propId == 0xc71f
                 || propId == 0xc725
                 || propId == 0xc726
                 || propId == 0xc740
                 || propId == 0xc741
                 || propId == 0xc74e
                 || propId == 0xc761
                 || propId == 0xc763
                 || propId == 0xc764
                 || propId == 0xc772
                 || propId == 0xc789
                 || propId == 0xc791
                 || propId == 0xc792
                 || propId == 0xc793
                 || propId == 0xc7a1
                 || propId == 0xc7a3
                 || propId == 0xc7a4
                 || propId == 0xc7a5
                 || propId == 0xc7a6
                 || propId == 0xc7a7
                 || propId == 0xc7a8
                 || propId == 0xc7aa
                 || propId == 0xc7b5
                 || propId == 0xc7d5
                 || propId == 0xc7e9
                 || propId == 0xc7ea
                 || propId == 0xc7eb
                 || propId == 0xc7ec
                 || propId == 0xc7ed
                 || propId == 0xc7ee
                 || propId == 0xcd2d
                 || propId == 0xcd2e
                 || propId == 0xcd30
                 || propId == 0xcd31
                 || propId == 0xcd32
                 || propId == 0xcd33
                 || propId == 0xcd34
                 || propId == 0xcd35
                 || propId == 0xcd36
                 || propId == 0xcd37
                 || propId == 0xcd38
                 || propId == 0xcd39
                 || propId == 0xcd3a
                 || propId == 0xcd3b
                 || propId == 0xea1c
                 || propId == 0xea1d
                 || propId == 0xfde8
                 || propId == 0xfde9
                 || propId == 0xfdea
                 || propId == 0xfe00
                 || propId == 0xfe4c
                 || propId == 0xfe4d
                 || propId == 0xfe4e
                 || propId == 0xfe51
                 || propId == 0xfe52
                 || propId == 0xfe53
                 || propId == 0xfe54
                 || propId == 0xfe55
                 || propId == 0xfe56
                 || propId == 0xfe57
                 || propId == 0xfe58
                 //iptc properties
                 || propId == 0x0105
                 || propId == 0x0114
                 || propId == 0x011E
                 || propId == 0x0128
                 || propId == 0x0132
                 || propId == 0x0146
                 || propId == 0x0150
                 || propId == 0x0200
                 || propId == 0x0203
                 || propId == 0x0204
                 || propId == 0x0205
                 || propId == 0x0207
                 || propId == 0x0208
                 || propId == 0x020A
                 || propId == 0x020C
                 || propId == 0x020F
                 || propId == 0x0214
                 || propId == 0x0216
                 || propId == 0x0219
                 || propId == 0x021A
                 || propId == 0x021B
                 || propId == 0x021E
                 || propId == 0x0223
                 || propId == 0x0225
                 || propId == 0x0226
                 || propId == 0x0228
                 || propId == 0x022A
                 || propId == 0x022D
                 || propId == 0x022F
                 || propId == 0x0232
                 || propId == 0x0237
                 || propId == 0x023C
                 || propId == 0x023E
                 || propId == 0x023F
                 || propId == 0x0241
                 || propId == 0x0246
                 || propId == 0x024B
                 || propId == 0x0250
                 || propId == 0x0255
                 || propId == 0x025A
                 || propId == 0x025C
                 || propId == 0x025F
                 || propId == 0x0264
                 || propId == 0x0265
                 || propId == 0x0267
                 || propId == 0x0269
                 || propId == 0x026E
                 || propId == 0x0273
                 || propId == 0x0274
                 || propId == 0x0276
                 || propId == 0x0278
                 || propId == 0x027A
                 || propId == 0x027D
                 || propId == 0x0282
                 || propId == 0x0283
                 || propId == 0x0287
                 || propId == 0x0296
                 || propId == 0x0297
                 || propId == 0x0298
                 || propId == 0x0299
                 || propId == 0x029A
                 || propId == 0x02C8
                 || propId == 0x02C9
                 || propId == 0x02CA
                 || propId == 0x0607

                )
                            {
                                image.RemovePropertyItem(propId);
                            }
                        }

                        // Save the image without metadata
                        image.Save($@"{destPath}\{fileName}.webp");
                        image.Dispose();
                    }
                }

                Console.WriteLine($"seconds : {stopWatch.Elapsed.TotalSeconds}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Provide path.");
            }
        }
    }
}

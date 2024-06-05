using MudBlazor;
using System;
using TomAndJerry.Model;

namespace TomAndJerry.DataBase
{
    public class Data
    {
        static Random random = new Random();
        static List<T> SuffeledArray<T>(List<T> array)
        {

            for (int i = 0; i < array.Count; i++)
            {
                var randIndex = random.Next(i, array.Count);
                var tempItem = array[randIndex];
                array[randIndex] = array[i];
                array[i] = tempItem;
            }
            return array;
        }
        public static Video GetVideo(string id)
        {
            return Videos.FirstOrDefault(x => x.VideoId == id) ?? Videos[0];
        }
        public static List<Video> GetRandomVideo()
        {
            return SuffeledArray(Videos);
        }
        public static List<Video> Videos = [
            new Video()
            {
                Id =001,
                Thumbnail ="image/thumbnail/001 - Puss Gets the Boot.mkv.jpg",
                Description="Puss Gets the Boot.mkv",
                VideoId="1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5",
                CommentName="Puss Gets the Boot.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =002,
                Thumbnail ="image/thumbnail/002 - The Midnight Snack.mkv.jpg",
                Description="The Midnight Snack.mkv",
                VideoId="13jTdN2m3LGmghBwKfOOwZe94KC_cUUve",
                CommentName="The Midnight Snack.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =003,
                Thumbnail ="image/thumbnail/003 - The Night Before Christmas.mkv.jpg",
                Description="The Night Before Christmas.mkv",
                VideoId="1FKdUFv-n6KwPEHfwuK3_LBgWDFSqseJZ",
                CommentName="The Night Before Christmas.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =004,
                Thumbnail ="image/thumbnail/004 - Fraidy Cat.mkv.jpg",
                Description="Fraidy Cat.mkv",
                VideoId="1LcnqXk4D4Pwed6QOTAeT2_jVGWQLR5o_",
                CommentName="Fraidy Cat.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =005,
                Thumbnail ="image/thumbnail/005 - Dog Trouble.mkv.jpg",
                Description="Dog Trouble.mkv",
                VideoId="1iwt1TOFfmII32BsP4sK_sOMuWUktcB2p",
                CommentName="Dog Trouble.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =006,
                Thumbnail ="image/thumbnail/006 - Puss N' Toots.mkv.jpg",
                Description="Puss N' Toots.mkv",
                VideoId="1Zg5gHe5sGpIV2zn4PA2wW20JeHKaCCzK",
                CommentName="Puss N' Toots.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =007,
                Thumbnail ="image/thumbnail/007 - The Bowling Alley-Cat.mkv.jpg",
                Description="The Bowling Alley-Cat.mkv",
                VideoId="1G-96ODL6g8bZdfy8aere_KhhNxZxiw8L",
                CommentName="The Bowling Alley-Cat.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =008,
                Thumbnail ="image/thumbnail/008 - Fine Feathered Friend.mkv.jpg",
                Description="Fine Feathered Friend.mkv",
                VideoId="1TS8mKeFV_AOtg6uHJuNDWtIUqc-IFBKn",
                CommentName="Fine Feathered Friend.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =009,
                Thumbnail ="image/thumbnail/009 - Sufferin' Cats!.mkv.jpg",
                Description="Sufferin' Cats!.mkv",
                VideoId="1gttrOzLJBBedS0wJ7rhsCJfc2oy5MaoF",
                CommentName="Sufferin' Cats!.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =010,
                Thumbnail ="image/thumbnail/010 - The Lonesome Mouse.mkv.jpg",
                Description="The Lonesome Mouse.mkv",
                VideoId="1KMQWZJWnx1iSOQh0PUjV6bRD6Btfbk0n",
                CommentName="The Lonesome Mouse.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =011,
                Thumbnail ="image/thumbnail/011 - The Yankee Doodle Mouse.mkv.jpg",
                Description="The Yankee Doodle Mouse.mkv",
                VideoId="1lNfAzf1iJm09rU3NKv79qIyAN35uN4CF",
                CommentName="The Yankee Doodle Mouse.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =012,
                Thumbnail ="image/thumbnail/012 - Baby Puss.mkv.jpg",
                Description="Baby Puss.mkv",
                VideoId="19q6g4RcjXrwYTkMbPHA7KFWst7cJ5k9I",
                CommentName="Baby Puss.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =013,
                Thumbnail ="image/thumbnail/013 - The Zoot Cat.mkv.jpg",
                Description="The Zoot Cat.mkv",
                VideoId="1Oi2nqteOcysyzviEKluxI_6p7sPedhzY",
                CommentName="The Zoot Cat.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =014,
                Thumbnail ="image/thumbnail/014 - The Million Dollar Cat.mkv.jpg",
                Description="The Million Dollar Cat.mkv",
                VideoId="1pCYuaNhXNokxEIRRKh1k6Zrm_wIas0Yc",
                CommentName="The Million Dollar Cat.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =015,
                Thumbnail ="image/thumbnail/015 - The Bodyguard.mkv.jpg",
                Description="The Bodyguard.mkv",
                VideoId="1ZejGOmc6s0Dc_9RskdHG_ttn3_2MB-lc",
                CommentName="The Bodyguard.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =016,
                Thumbnail ="image/thumbnail/016 - Puttin' on the Dog.mkv.jpg",
                Description="Puttin' on the Dog.mkv",
                VideoId="1xinrB5mtFDwV99nvjVU-Ls1-3sHzlkcZ",
                CommentName="Puttin' on the Dog.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =017,
                Thumbnail ="image/thumbnail/017 - Mouse Trouble.mkv.jpg",
                Description="Mouse Trouble.mkv",
                VideoId="1al6fkt6Dr_V700ugZtWYtt9D02y0-0CD",
                CommentName="Mouse Trouble.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =018,
                Thumbnail ="image/thumbnail/018 - The Mouse Comes to Dinner.mkv.jpg",
                Description="The Mouse Comes to Dinner.mkv",
                VideoId="1J6rSC4zrkOfbY8BCbEeK_2bCkPV2EHiI",
                CommentName="The Mouse Comes to Dinner.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =019,
                Thumbnail ="image/thumbnail/019 - Mouse in Manhattan.mkv.jpg",
                Description="Mouse in Manhattan.mkv",
                VideoId="1EofX3jICUWQfdI_4JwrMdbj9Pu5tzWJJ",
                CommentName="Mouse in Manhattan.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =020,
                Thumbnail ="image/thumbnail/020 - Tee for Two.mkv.jpg",
                Description="Tee for Two.mkv",
                VideoId="14p5TAqPAsX0d8GmzR0F2tPiL5FvCLcIp",
                CommentName="Tee for Two.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =021,
                Thumbnail ="image/thumbnail/021 - Flirty Birdy.mkv.jpg",
                Description="Flirty Birdy.mkv",
                VideoId="1RTyTETXKyH9YFQSlFotGgdsTIClqB0wA",
                CommentName="Flirty Birdy.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =022,
                Thumbnail ="image/thumbnail/022 - Quiet Please!.mkv.jpg",
                Description="Quiet Please!.mkv",
                VideoId="1Ulwx2duCOowPM_JFCd7bWyHBEvCLSVej",
                CommentName="Quiet Please!.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =023,
                Thumbnail ="image/thumbnail/023 - Springtime for Thomas.mkv.jpg",
                Description="Springtime for Thomas.mkv",
                VideoId="1UOxY3ULpNEc0fXX9-sP5tS0G6y-_aHBs",
                CommentName="Springtime for Thomas.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =024,
                Thumbnail ="image/thumbnail/024 - The Milky Waif.mkv.jpg",
                Description="The Milky Waif.mkv",
                VideoId="1tRUALwP7z6KKdwZeW6e_faXRq3abXG8T",
                CommentName="The Milky Waif.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =025,
                Thumbnail ="image/thumbnail/025 - Trap Happy.mkv.jpg",
                Description="Trap Happy.mkv",
                VideoId="1caxZYthBzdjGPhJaUlquBrPGM6jnkaG2",
                CommentName="Trap Happy.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =026,
                Thumbnail ="image/thumbnail/026 - Solid Serenade.mkv.jpg",
                Description="Solid Serenade.mkv",
                VideoId="1CAIOrLJ9T29ALSs8YIHEnyLFEhhWTytG",
                CommentName="Solid Serenade.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =027,
                Thumbnail ="image/thumbnail/027 - Cat Fishin'.mkv.jpg",
                Description="Cat Fishin'.mkv",
                VideoId="1-GCDdH6Ub5UiyBRnSamfaTGA675xEscX",
                CommentName="Cat Fishin'.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =028,
                Thumbnail ="image/thumbnail/028 - Part Time Pal.mkv.jpg",
                Description="Part Time Pal.mkv",
                VideoId="1p7_fTId15QaoslPXBFt3tEmOER90pCqh",
                CommentName="Part Time Pal.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =029,
                Thumbnail ="image/thumbnail/029 - The Cat Concerto.mkv.jpg",
                Description="The Cat Concerto.mkv",
                VideoId="1frwIj53s2_Ca_a68J3t2iEB34u6A3Wsv",
                CommentName="The Cat Concerto.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =030,
                Thumbnail ="image/thumbnail/030 - Dr. Jekyll and Mr. Mouse.mkv.jpg",
                Description="Dr. Jekyll and Mr. Mouse.mkv",
                VideoId="11z4WbQPspthTJ1zuv0dSYYdKZGz6_FwC",
                CommentName="Dr. Jekyll and Mr. Mouse.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =031,
                Thumbnail ="image/thumbnail/031 - Salt Water Tabby.mkv.jpg",
                Description="Salt Water Tabby.mkv",
                VideoId="1iGkvjwAQkZw3aeqQvl4dQPidyvZOBnLu",
                CommentName="Salt Water Tabby.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =032,
                Thumbnail ="image/thumbnail/032 - A Mouse in the House.mkv.jpg",
                Description="A Mouse in the House.mkv",
                VideoId="1H9Aj_w5iEI_jcJW-E_NPEO_aF2sBuIbJ",
                CommentName="A Mouse in the House.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =033,
                Thumbnail ="image/thumbnail/033 - The Invisible Mouse.mkv.jpg",
                Description="The Invisible Mouse.mkv",
                VideoId="1JJwojenokyYoTkzOfr87bYCCU-BzUfad",
                CommentName="The Invisible Mouse.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =034,
                Thumbnail ="image/thumbnail/034 - Kitty Foiled.mkv.jpg",
                Description="Kitty Foiled.mkv",
                VideoId="1_pjmqur4qnUx_eMlLN7IFtTDsgrGrzlu",
                CommentName="Kitty Foiled.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =035,
                Thumbnail ="image/thumbnail/035 - The Truce Hurts.mkv.jpg",
                Description="The Truce Hurts.mkv",
                VideoId="1cAt8KWM1yJooRRHOv3XVO23ssgBC8z-v",
                CommentName="The Truce Hurts.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =036,
                Thumbnail ="image/thumbnail/036 - Old Rockin' Chair Tom.mkv.jpg",
                Description="Old Rockin' Chair Tom.mkv",
                VideoId="1e3JsQuxqwvMFt9hycEJ6osVEk0XTt6Hy",
                CommentName="Old Rockin' Chair Tom.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =037,
                Thumbnail ="image/thumbnail/037 - Professor Tom.mkv.jpg",
                Description="Professor Tom.mkv",
                VideoId="1MRqRtGiqO7JqI0xh71FtriYoOLgDDztI",
                CommentName="Professor Tom.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =038,
                Thumbnail ="image/thumbnail/038 - Mouse Cleaning.mkv.jpg",
                Description="Mouse Cleaning.mkv",
                VideoId="14myUm809nZCW3h0wHe-264HODNbA1y9E",
                CommentName="Mouse Cleaning.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =039,
                Thumbnail ="image/thumbnail/039 - Polka-Dot Puss.mkv.jpg",
                Description="Polka-Dot Puss.mkv",
                VideoId="174I88wu7_F8194WPgb_yThodi6sN8uKd",
                CommentName="Polka-Dot Puss.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =040,
                Thumbnail ="image/thumbnail/040 - The Little Orphan.mkv.jpg",
                Description="The Little Orphan.mkv",
                VideoId="1t4TVIvTQOo9nBdKo4-EZCQxkylOOs3jh",
                CommentName="The Little Orphan.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =041,
                Thumbnail ="image/thumbnail/041 - Hatch Up Your Troubles.mkv.jpg",
                Description="Hatch Up Your Troubles.mkv",
                VideoId="1Pp-nAmCt36WhS5wyTzkIvju5b2SsJUKX",
                CommentName="Hatch Up Your Troubles.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =042,
                Thumbnail ="image/thumbnail/042 - Heavenly Puss.mkv.jpg",
                Description="Heavenly Puss.mkv",
                VideoId="1oZl240zu4iuXz_9d3Oe7j4zO6n8Ag_Fc",
                CommentName="Heavenly Puss.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =043,
                Thumbnail ="image/thumbnail/043 - The Cat and the Mermouse.mkv.jpg",
                Description="The Cat and the Mermouse.mkv",
                VideoId="13v21mpCt4VyWigLIhVDfhSttXBkDguow",
                CommentName="The Cat and the Mermouse.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =044,
                Thumbnail ="image/thumbnail/044 - Love that Pup.mkv.jpg",
                Description="Love that Pup.mkv",
                VideoId="1Ixi0UkfDhBS84OtIifuhyYpnVZcvypHM",
                CommentName="Love that Pup.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =045,
                Thumbnail ="image/thumbnail/045 - Jerry's Diary.mkv.jpg",
                Description="Jerry's Diary.mkv",
                VideoId="1toh8ievg1Ytx_JarzL6q0vFAp_MuTObL",
                CommentName="Jerry's Diary.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =046,
                Thumbnail ="image/thumbnail/046 - Tennis Chumps.mkv.jpg",
                Description="Tennis Chumps.mkv",
                VideoId="1ESURU4mLCwCm9VBOb8RqJ6_0WCjFI1pw",
                CommentName="Tennis Chumps.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =047,
                Thumbnail ="image/thumbnail/047 - Little Quacker.mkv.jpg",
                Description="Little Quacker.mkv",
                VideoId="1IqiZecYaqv3x7JdcNcptzyj-b9brE5cV",
                CommentName="Little Quacker.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =048,
                Thumbnail ="image/thumbnail/048 - Saturday Evening Puss.mkv.jpg",
                Description="Saturday Evening Puss.mkv",
                VideoId="1b05dZ4O_x11Sdz_T4aRVkX9Xke8auIAl",
                CommentName="Saturday Evening Puss.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =049,
                Thumbnail ="image/thumbnail/049 - Texas Tom.mkv.jpg",
                Description="Texas Tom.mkv",
                VideoId="1MYz5D_YUrqdXC4BKwXDu5cmOKWF1jURq",
                CommentName="Texas Tom.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            },
            new Video()
            {
                Id =050,
                Thumbnail ="image/thumbnail/050 - Jerry and the Lion.mkv.jpg",
                Description="Jerry and the Lion.mkv",
                VideoId="1FewEs9QkGFawGYl873ax1LljbipiQ3x0",
                CommentName="Jerry and the Lion.mkv",
                VideoUrl="https://drive.google.com/file/d/1MW9aL5wSd9MsMouvlZgTErvWXJ4KctS5/preview"
            }


        ];
         
        
    }
}

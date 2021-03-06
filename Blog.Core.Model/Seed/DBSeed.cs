﻿using Blog.Core.Model.Models;
using SqlSugar;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    public class DBSeed
    {
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext)
        {
            try
            {
                // 注意！一定要手动先创建要给空的数据库
                // 会覆盖，可以设置为true，来备份数据
                // 如果生成过了，第二次，就不用再执行一遍了,注释掉该方法即可
                myContext.CreateTableByEntity(false, typeof(Advertisement), typeof(BlogArticle), typeof(Guestbook), typeof(Module), typeof(ModulePermission), typeof(OperateLog), typeof(PasswordLib), typeof(Permission), typeof(Role), typeof(RoleModulePermission), typeof(sysUserInfo), typeof(Topic), typeof(TopicDetail), typeof(UserRole));

                // 后期单独处理某些表
                //myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));
                //myContext.Db.CodeFirst.InitTables(typeof(Permission)); 


                #region Advertisement
                if (!await myContext.Db.Queryable<Advertisement>().AnyAsync())
                {
                    myContext.GetEntityDB<Advertisement>().Insert(
                       new Advertisement()
                       {
                           Createdate = DateTime.Now,
                           Remark = "mark",
                           Title = "good"
                       });
                }
                #endregion

                #region BlogArticle Guestbook
                if (!await myContext.Db.Queryable<BlogArticle>().AnyAsync())
                {
                    int bid = myContext.GetEntityDB<BlogArticle>().InsertReturnIdentity(
                         new BlogArticle()
                         {
                             bsubmitter = "admins",
                             btitle = "老张的哲学",
                             bcategory = "技术博文",
                             bcontent = "<p>1，ctrl+alt+delete 去修改密码</p><p>2、<span style='color: inherit; '>打开“服务器管理器”，选择“配置”-“本地用户和组”-“用户，</span><span style='color: inherit; '>右击administrator，选择“属性”，在“常规”选项中勾上“密码永不过期”，点击“应用”和“确定”。</span><span style='color: inherit; '><br></span></p><p><span style='color: inherit; '>3、</span><span style='color: inherit; '>在开始菜单中选择“管理工具”-“本地安全策略”，</span><span style='color: inherit; '>选择“安全策略”-“账户策略”-“密码策略”，编辑“密码最短使用期限”和“密码最长使用期限”，天数设置为0，即永不过期，点击“确定”即可。</span><span style='color: inherit; '><br></span></p><p><br></p>",
                             btraffic = 1,
                             bcommentNum = 0,
                             bUpdateTime = DateTime.Now,
                             bCreateTime = DateTime.Now
                         });

                    if (bid > 0)
                    {

                        if (!await myContext.Db.Queryable<Guestbook>().AnyAsync())
                        {
                            myContext.GetEntityDB<Guestbook>().Insert(
                               new Guestbook()
                               {
                                   blogId = bid,
                                   createdate = DateTime.Now,
                                   username = "user",
                                   phone = "110",
                                   QQ = "100",
                                   body = "很不错",
                                   ip = "127.0.0.1",
                                   isshow = true,
                               });
                        }
                    }
                }
                #endregion

                #region Module
                int mid = 0;
                if (!await myContext.Db.Queryable<Module>().AnyAsync())
                {
                    mid = myContext.GetEntityDB<Module>().InsertReturnIdentity(
                        new Module()
                        {
                            IsDeleted = false,
                            Name = "values的接口信息",
                            LinkUrl = "/api/values",
                            OrderSort = 1,
                            IsMenu = false,
                            Enabled = true,
                            CreateTime = DateTime.Now,
                        });

                }
                #endregion

                #region Role
                int rid = 0;
                if (!await myContext.Db.Queryable<Role>().AnyAsync())
                {
                    rid = myContext.GetEntityDB<Role>().InsertReturnIdentity(
                        new Role()
                        {
                            IsDeleted = false,
                            Name = "Admin",
                            Description = "我是一个admin管理员",
                            OrderSort = 1,
                            CreateTime = DateTime.Now,
                            Enabled = true,
                            ModifyTime = DateTime.Now
                        });

                }
                #endregion

                #region RoleModulePermission
                if (mid > 0 && rid > 0)
                {
                    if (!await myContext.Db.Queryable<RoleModulePermission>().AnyAsync())
                    {
                        myContext.GetEntityDB<RoleModulePermission>().Insert(
                            new RoleModulePermission()
                            {
                                IsDeleted = false,
                                RoleId = rid,
                                ModuleId = mid,
                                CreateTime = DateTime.Now,
                                ModifyTime = DateTime.Now
                            });
                    }
                }
                #endregion

                #region sysUserInfo
                int uid = 0;
                if (!await myContext.Db.Queryable<sysUserInfo>().AnyAsync())
                {
                    uid = myContext.GetEntityDB<sysUserInfo>().InsertReturnIdentity(
                        new sysUserInfo()
                        {
                            uLoginName = "admins",
                            uLoginPWD = "admins",
                            uRealName = "admins",
                            uStatus = 0,
                            uCreateTime = DateTime.Now,
                            uUpdateTime = DateTime.Now,
                            uLastErrTime = DateTime.Now,
                            uErrorCount = 0

                        });

                }
                #endregion

                #region UserRole
                if (uid > 0 && rid > 0)
                {
                    if (!await myContext.Db.Queryable<UserRole>().AnyAsync())
                    {
                        myContext.GetEntityDB<UserRole>().Insert(
                            new UserRole()
                            {
                                IsDeleted = false,
                                UserId = uid,
                                RoleId = rid,
                                CreateTime = DateTime.Now,
                                ModifyTime = DateTime.Now
                            });
                    }
                }
                #endregion

                #region Topic TopicDetail
                if (!await myContext.Db.Queryable<Topic>().AnyAsync())
                {
                    int tid = myContext.GetEntityDB<Topic>().InsertReturnIdentity(
                         new Topic()
                         {
                             tLogo = "/Upload/20180626/95445c8e288e47e3af7a180b8a4cc0c7.jpg",
                             tName = "《罗马人的故事》",
                             tDetail = "这是一个荡气回肠的故事",
                             tIsDelete = false,
                             tRead = 0,
                             tCommend = 0,
                             tGood = 0,
                             tCreatetime = DateTime.Now,
                             tUpdatetime = DateTime.Now,
                             tAuthor = "laozhang"
                         });

                    if (tid > 0)
                    {

                        if (!await myContext.Db.Queryable<TopicDetail>().AnyAsync())
                        {
                            myContext.GetEntityDB<TopicDetail>().Insert(
                               new TopicDetail()
                               {
                                   TopicId = tid,
                                   tdLogo = "/Upload/20180627/7548de20944c45d48a055111b5a6c1b9.jpg",
                                   tdName = "第一章　罗马的诞生 第一节　传说的年代",
                                   tdContent = "<p>第一节　传说的年代</p><p>每个民族都有自己的神话传说。大概希望知道本民族的来源是个很自然的愿望吧。但这是一个难题，因为这几乎不可能用科学的方法来解释清楚。不过所有的民族都没有这样的奢求。他们只要有一个具有一定的条理性，而又能振奋其民族精神的浪漫故事就行，别抬杠，象柏杨那样将中国的三皇五帝都来个科学分析，来评论他们的执政之优劣是大可不必的。</p><p>对於罗马人，他们有一个和特洛伊城的陷落相关的传说。</p><p>位於小亚细亚西岸的繁荣的城市特洛伊，在遭受了阿加美农统帅的希腊联军的十年围攻之後，仍未陷落。希腊联军於是留下一个巨大的木马後假装撤兵。特洛伊人以为那是希腊联军留给自己的礼物，就将它拉入城内。</p><p>当庆祝胜利的狂欢结束，特洛伊人满怀对明日的和平生活的希望熟睡後，藏在木马内的希腊士兵一个又一个地爬了出来。就在这天夜里，特洛伊城便在火光和叫喊中陷落了。全城遭到大屠杀 ，幸免於死的人全都沦为奴隶。混乱之中只有特洛伊国王的驸马阿伊尼阿斯带着老父，儿子等数人在女神维娜斯的帮助下成功地逃了出来。这驸马爷乃是女神维娜斯与凡人男子之间的儿子，女神维娜斯不忍心看着自己的儿子被希腊士兵屠杀 。</p><p>这阿驸马一行人分乘几条船，离开了火光冲天的特洛伊城。在女神维娜斯的指引下，浪迹地中海，最後在意大利西岸登陆。当地的国王看上了阿伊尼阿斯并把自己的女儿嫁给了他。他又是驸马了，与他的新妻过起了幸福的生活。难民们也安定了下来。</p><p>阿伊尼阿斯死後，跟随他逃难来的儿子继承了王位。新王在位三十年後，离开了这块地方，到台伯河(Tiber)下游建了一个新城亚尔巴龙迦城。这便是罗马城的前身了。</p><p>罗马人自古相信罗马城是公元前731年4月21日由罗莫路和勒莫(Romulus and Remus)建设的。而这两个孪生兄弟是从特洛伊逃出的阿伊尼阿斯的子孙。後来，罗马人接触了希腊文化後才知道特洛伊的陷落是在公元前十三世纪，老早的事了。罗马人好象并没有对这段空白有任何烦恼，随手编出一串传说，把那空白给填补了。反正传说这事荒唐一点的更受欢迎。经过了一堆搞不清谁是谁的王的统治，出现了一个什麽王的公主。</p><p>公主的叔父在篡夺了王位後，为了防止公主结婚生子威胁自己的王位，便任命未婚的公主为巫女。这是主管祭神的职位，象修女一样不得结婚。</p><p>不巧一日这美丽的公主在祭事的空余，来到小河边午睡。也是合当有事，被过往的战神玛尔斯(Mars)一见钟情。这玛尔斯本是靠挑起战争混饭吃的，但也常勾引 良家妇女。这天战神也没错过机会，立刻由天而降，与公主一试云雨。据说战神的技术特神，公主还没来得及醒便完事升天去了。後来公主生了一双胞胎，起名罗莫路和勒莫。</p><p>叔父闻知此事大怒，将公主投入大牢，又把那双胞胎放在篮子里抛入台伯河，指望那篮子漂入大海将那双胞胎淹死。类似的故事在旧约圣经里也有，那是关於摩西的事，好象这类传说在当地十分流行。</p><p>再说那兄弟俩的篮子被河口附近茂密的灌木丛钩住而停了下来，俩人哭声引来的一只过路的母狼。意大利的狼都带点慈悲心，不但没吃了俩人当点心，还用自己的奶去喂他们，这才救了俩小命。</p><p>不过，总是由狼养活也没法交&nbsp;待，於是又一日一放羊的在这地盘上溜哒，发现了兄弟俩，将他们抱了回去扶养成人 。据说现在这一带仍有许多放羊的。</p><p>兄弟俩长大後成了这一带放羊人的头，在与别的放羊人的圈子的打斗中不断地扩展自己的势力范围。圈子大了，情报也就多了，终于有一天，罗莫路和勒莫知道了自己身事。</p><p>兄弟俩就带着手下的放羊人呼啸着去打破了亚尔巴龙迦城，杀了那国王，将王位又交&nbsp;还给了自己祖父。他们的母亲似乎已经死在了大牢里。但兄弟俩也没在亚尔巴龙迦城多住，他们认为亚尔巴龙迦城位於山地，虽然易守难攻，却不利发展。加上兄弟俩是在台伯河的下游长大的，所以便回到原地，建了个新城。除了手下的放羊人又加上了附近的放羊人和农民。</p><p>消灭了共同的敌人後，兄弟俩的关系开始恶化。有人说是为了新城的命名，有人说是为了新城的城址，也有人说是为了争夺王位。兄弟俩於是分割统治，各占一小山包。但纷争又开始了，勒莫跳过了罗莫路为表示势力范围而挖的沟。对於这种侵犯他人权力的行为，罗莫路大义灭亲地在自己兄弟的後脑上重重地来了一锄头，勒莫便被灭了。</p><p></p><p>於是这城便以罗莫路的名字命名为罗马，这就是公元前731年4月21日的事了，到现在这天仍是意大利的节日，罗马人会欢天喜地的庆祝罗莫路杀了自己的…不，是庆祝罗马建城。王位当然也得由罗莫路来坐，一切问题都没了。这时四年一度的奥林匹克运动会在希腊已经开了六回，罗马也从传说的时代走出，近入了历史时代。</p><p><br></p>",
                                   tdDetail = "第一回",
                                   tdIsDelete = false,
                                   tdRead = 1,
                                   tdCommend = 0,
                                   tdGood = 0,
                                   tdCreatetime = DateTime.Now,
                                   tdUpdatetime = DateTime.Now,
                                   tdTop = 0,
                               });
                        }
                    }
                }
                #endregion



            }
            catch (Exception ex)
            {
                throw new Exception("注意要先创建空的数据库");
            }
        }
    }
}

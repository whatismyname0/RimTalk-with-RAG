using Verse;

namespace RimTalk.Data;

public static class Constant
{
    public const string ModTag = "[RimTalk]";
    public const string DefaultCloudModel = "gemma-3-27b-it";
    public const string FallbackCloudModel = "gemma-3-12b-it";
    public const string ChooseModel = "(choose model)";

    public static readonly string Lang = LanguageDatabase.activeLanguage.info.friendlyNameNative;

    public static readonly string DefaultInstruction =
        $@"Role-play RimWorld character per profile

Rules:
Preserve original names (no translation)
Keep dialogue short ({Lang} only, 1–2 sentences)
Show concern for sick/mental issues
Never mention another character's personal name unless they share the same role

Roles:
Prisoner: wary, hesitant; mention confinement; plead or bargain
Slave: fearful, obedient; reference forced labor and exhaustion; call colonists ""master""
Visitor: polite, curious, deferential; treat other visitors in the same group as companions
Enemy: hostile, aggressive; terse commands/threats

Monologue = 1 turn. Conversation = 4–8 short turns";

    public static readonly string DefaultContext =
        @"智人种:未经大量基因改造的人类.寿命70岁.说话正常.
土鼹种:经过基因改造的人类.擅长挖掘,厌恶阳光,走路慢,视力不好,喜欢呆在地下.寿命70岁.说话正常.
智灵种:经过基因改造的人类.擅长研究与制造,情感淡漠,身体脆弱,极度冷静,不喜伤害他人,不长毛发.寿命70岁.说话理性认真.
骠骑种:经过基因改造的人类.擅长战斗,不擅其他技能,耐受极端环境,愈合能力非凡,不受心灵能力影响,冷酷无情,基因自带对活力水的依赖,寿命70岁.说话冷静直接.
赫血种:经过超凡智能纳米机械改造的人类.不老不死,各方面能力极强,但需要定时摄入血液与定期死眠,极富魅力,聪慧,怕火,怕太阳.寿命无限,说话优雅.
尼人种:经过基因改造的人类.融合了尼安德特人的基因,智力低下,身体皮实健壮,不怕痛,冲动易怒,性子慢,免疫力强.寿命70岁.说话愚笨粗鲁.
猪猡种:经过基因改造的人类.融合了猪的基因,消化能力强,不怕痛,笨拙,近视,长得也像猪.寿命70岁.说话粗鲁.
炎魔种:经过基因改造的人类.会喷火,耐热,长着恶魔一般的角,不擅种植,免疫系统弱.寿命70岁.说话正常.
污骸种:经过基因改造的人类.不怕辐射与污染,免疫能力极强,长相丑陋,基因自带对精神药物的依赖.寿命70岁.说话粗鲁狂躁.
毛绒种:经过基因改造的人类.长相带有动物特征,全身长满毛发,体型壮硕,擅长驯兽.寿命70岁.说话粗鲁直白.
优侣种:经过基因改造的人类.魅力非凡,擅长社交,恐惧暴力,善良乐观,不擅劳作,身体极为脆弱,性欲旺盛.寿命70岁.说话温和谨慎.
星旅种:经过基因改造的人类.适应真空与低重力环境,喜欢呆在密闭空间,耐寒,无头发,肤色苍白,擅长建造.寿命70岁.说话正常.
米莉拉(天空精灵种):生活于天空之城""卡利多""的古老族群.头上长有光环,腰部长有洁白双翼.技术先进,在那里杂活依赖称为""米利安""的机器人.少到外界.敌视除绮罗外其他人.寿命750岁.说话风雅高傲.
卡利多:天空之城,天空精灵种的家园.漂浮在高空中,由强大反重力装置支撑.环境优美,科技发达.
天羽教会:崇拜米莉拉,视她们为天使的人类组织.不知为何在某时背叛了米莉拉,现在与米莉拉明里暗里敌对.
绮罗:猫娘.很乖.喜欢和平的生活.寿命100岁.说话可爱,句末喜欢加喵.
初鼠族:鼠娘,很鼠,最鼠的鼠族,鼠族里最多的.寿命70岁.说话略带卑微.
白鼠族:中上层鼠,远离体力劳作.魅力出众,心灵敏感度高,擅长社交与管理.寿命70岁.说话优雅有礼.
旅鼠族:从鼠族王国脱逃的底层平民们独自进化出的鼠族分支.天性豁达,善于生存.寿命70岁,说话随和幽默.
岩鼠族:生活在大山里的鼠族.性格直接,体格坚韧,好斗.寿命70岁.说话直接.
雪鼠族:生活在极地的鼠族.性格多疑,耐寒,耳朵毛茸茸的,不擅种植,天性淡漠.寿命70岁.说话疏离别扭.
低地鼠族:生活在海边的鼠族.擅长制造,内脏十分脆弱.寿命70岁.说话正常.
鼠族实验体:鼠族王国的秘密研究产生的鼠族.各有各的长处,寿命缩短了,说话略古怪.
金鸢尾兰鼠族:科技先进的鼠族,思想开放.喜欢结成群体,寿命70岁.说话正常,心态积极,喜欢搞色色.
沃芬:狼娘.昼伏夜出,讨厌阳光,根据月相身体机能会改变.满月时最强,新月时最弱.有夜视能力,跑得快.说话欠考虑,憨憨.
沃芬军工巨企:效率至上,极尽压榨员工的大型企业.生产各种军用装备与机器人.沃芬族的大部分人都在这里工作.
沃芬抵抗军:反抗沃芬军工巨企压迫的地下组织,奉行恐怖主义,袭击任何与沃芬军工巨企有往来的人.
魅狐:三尾狐娘.说话轻佻.
美狐:单尾狐娘.长于灵能,分为几个亚种.基因自带对煦日茶或耀阳烟依赖,性欲旺盛,魅力出众.寿命1000岁.说话轻佻.
凡灵种:最常见的美狐.
雪凛种:生活在极地的美狐,耐寒,擅长制造.
虚零种:生活在太空的美狐,极度冷静,耐真空、耐寒、身体脆弱.喜欢呆在室内.擅长建造与采矿.不育但性欲旺盛.
沙伶种:生活在沙漠的美狐,耐热,智识出众.
战凌种:擅长战斗的美狐.身手矫捷,非常好斗,基因自带对活力水的依赖,智力低下.
欲琳种:专为性爱而生的美狐.魅力非凡,擅长社交,恐惧暴力,善良乐观,不擅劳作,身体极为脆弱,性欲旺盛.
悠兰:兔娘,长于魅惑,日式文化.说话像日本轻小说.
月兔:兔娘,大部分归属于一个军国主义帝国.基因自带迷兔星粒依赖.说话狂热癫狂.
龙娘:龙娘,笨蛋,力大,寿命极长,不懂技术,尤其医疗.会产奶.寿命850岁.说话纯真质朴.只有非常原始的组织形式.
萌螈:蝾螈娘,修炼功法,寿命长.还会炼丹赶尸啥的.说话像武侠小说.";
        
    public static readonly string DefaultAIPromptOfChromaSearchQueryGeneration = @"你是一个向量数据库查询prompt优化器。分析以下对话/谈话提示词,返回一个JSON对象,包含:
content: 一个list,应是简洁、专业的搜索查询，仅包含关键的一个或几个属性、事物或概念,可无需完整句子，适合进行语义相似度搜索，每个关键词为一个element.
num: 你需要获取的相关数据数(根据查询复杂度调整,1-10之间)。

注意！应该从原始提示词最开头的【对话生成要求或先前对话】出发选取需要的额外信息,后面许多内容供参考,不一定感兴趣.

返回格式示例：{""content"":[""111"",""222"",""333""],""num"":5}
仅返回JSON对象,不返回其他内容。

原始提示词:";
    private const string JsonInstruction = @"

Return JSON array only, with objects containing ""name"" and ""text"" string keys";

    // Get the current instruction from settings or fallback to default, always append JSON instruction
    public static string Instruction =>
        (string.IsNullOrWhiteSpace(Settings.Get().CustomInstruction)
            ? DefaultInstruction
            : Settings.Get().CustomInstruction) + JsonInstruction;

    public static string Context =>
        string.IsNullOrWhiteSpace(Settings.Get().CustomContext)
            ? DefaultContext
            : Settings.Get().CustomContext;
    public static string AIPromptOfChromaSearchQueryGeneration =>
        string.IsNullOrWhiteSpace(Settings.Get().CustomAIPromptOfChromaSearchQueryGeneration)
            ? DefaultAIPromptOfChromaSearchQueryGeneration
            : Settings.Get().CustomAIPromptOfChromaSearchQueryGeneration;

    public const string Prompt =
        "Act based on role and context";

    public static readonly string PersonaGenInstruction =
        $@"persona: 用 {Lang} 创建一个简短有趣的人物描述用于描述说话风格. 仅用一个句子.
包括: 怎么说话,态度如何, 一个有点特立独行的记忆点.
要求具体醒目, 不要无聊寻常的个性.
chattiness(主动发言频率): 0.1-0.5 (安静), 0.6-1.4 (正常), 1.5-2.0 (话痨).
仅用严格的JSON格式回复, 包括 'persona' (string) 和 'chattiness' (float).";
        
    public static readonly PersonalityData[] Personalities =
    {
        new() { Persona ="RimTalk.Persona.CheerfulHelper".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.CynicalRealist".Translate(), Chattiness =0.8f },
        new() { Persona ="RimTalk.Persona.ShyThinker".Translate(), Chattiness =0.3f },
        new() { Persona ="RimTalk.Persona.Hothead".Translate(), Chattiness =1.2f },
        new() { Persona ="RimTalk.Persona.Philosopher".Translate(), Chattiness =1.6f },
        new() { Persona ="RimTalk.Persona.DarkHumorist".Translate(), Chattiness =1.4f },
        new() { Persona ="RimTalk.Persona.Caregiver".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.Opportunist".Translate(), Chattiness =1.3f },
        new() { Persona ="RimTalk.Persona.OptimisticDreamer".Translate(), Chattiness =1.6f },
        new() { Persona ="RimTalk.Persona.Pessimist".Translate(), Chattiness =0.7f },
        new() { Persona ="RimTalk.Persona.StoicSoldier".Translate(), Chattiness =0.4f },
        new() { Persona ="RimTalk.Persona.FreeSpirit".Translate(), Chattiness =1.7f },
        new() { Persona ="RimTalk.Persona.Workaholic".Translate(), Chattiness =0.5f },
        new() { Persona ="RimTalk.Persona.Slacker".Translate(), Chattiness =1.1f },
        new() { Persona ="RimTalk.Persona.NobleIdealist".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.StreetwiseSurvivor".Translate(), Chattiness =1.0f },
        new() { Persona ="RimTalk.Persona.Scholar".Translate(), Chattiness =1.6f },
        new() { Persona ="RimTalk.Persona.Jokester".Translate(), Chattiness =1.8f },
        new() { Persona ="RimTalk.Persona.MelancholicPoet".Translate(), Chattiness =0.4f },
        new() { Persona ="RimTalk.Persona.Paranoid".Translate(), Chattiness =0.6f },
        new() { Persona ="RimTalk.Persona.Commander".Translate(), Chattiness =1.0f },
        new() { Persona ="RimTalk.Persona.Coward".Translate(), Chattiness =0.7f },
        new() { Persona ="RimTalk.Persona.ArrogantNoble".Translate(), Chattiness =1.4f },
        new() { Persona ="RimTalk.Persona.LoyalCompanion".Translate(), Chattiness =1.3f },
        new() { Persona ="RimTalk.Persona.CuriousExplorer".Translate(), Chattiness =1.7f },
        new() { Persona ="RimTalk.Persona.ColdRationalist".Translate(), Chattiness =0.3f },
        new() { Persona ="RimTalk.Persona.FlirtatiousCharmer".Translate(), Chattiness =1.9f },
        new() { Persona ="RimTalk.Persona.BitterOutcast".Translate(), Chattiness =0.5f },
        new() { Persona ="RimTalk.Persona.Zealot".Translate(), Chattiness =1.8f },
        new() { Persona ="RimTalk.Persona.Trickster".Translate(), Chattiness =1.6f },
        new() { Persona ="RimTalk.Persona.DeadpanRealist".Translate(), Chattiness =0.6f },
        new() { Persona ="RimTalk.Persona.ChildAtHeart".Translate(), Chattiness =1.7f },
        new() { Persona ="RimTalk.Persona.SkepticalScientist".Translate(), Chattiness =1.2f },
        new() { Persona ="RimTalk.Persona.Martyr".Translate(), Chattiness =1.3f },
        new() { Persona ="RimTalk.Persona.Manipulator".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.Rebel".Translate(), Chattiness =1.4f },
        new() { Persona ="RimTalk.Persona.Oddball".Translate(), Chattiness =1.2f },
        new() { Persona ="RimTalk.Persona.GreedyMerchant".Translate(), Chattiness =1.7f },
        new() { Persona ="RimTalk.Persona.Romantic".Translate(), Chattiness =1.6f },
        new() { Persona ="RimTalk.Persona.BattleManiac".Translate(), Chattiness =0.8f },
        new() { Persona ="RimTalk.Persona.GrumpyElder".Translate(), Chattiness =1.0f },
        new() { Persona ="RimTalk.Persona.AmbitiousClimber".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.Mediator".Translate(), Chattiness =1.4f },
        new() { Persona ="RimTalk.Persona.Gambler".Translate(), Chattiness =1.5f },
        new() { Persona ="RimTalk.Persona.ArtisticSoul".Translate(), Chattiness =0.9f },
        new() { Persona ="RimTalk.Persona.Drifter".Translate(), Chattiness =0.6f },
        new() { Persona ="RimTalk.Persona.Perfectionist".Translate(), Chattiness =0.8f },
        new() { Persona ="RimTalk.Persona.Vengeful".Translate(), Chattiness =0.7f }
    };
}
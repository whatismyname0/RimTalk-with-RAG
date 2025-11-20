# RimTalk ChromaDB 集成指南

## 概述

RimTalk 现已集成 ChromaDB 向量数据库，用于存储和查询对话历史，以增强对话上下文的相关性。

## 架构

### Python 层 (`Source/ChromaManager/`)

- **ChromaManager.py**: 核心 Python 模块，管理：
  - 存档专有数据库实例（per-save）
  - 使用 BGE-M3 模型进行对话向量化
  - 相关性查询和元数据过滤
  - 自动清理机制（800k 条目上限）

### C# 层 (`Source/Service/`)

- **ChromaService.cs**: 高级接口层
  - 初始化/关闭存档连接
  - 异步存储对话
  - 异步查询历史上下文
  
- **ChromaClient.cs**: IPC 通信层
  - 通过 stdio 与 Python 进程通信
  - JSON 序列化/反序列化
  - 超时和错误处理

- **ChromaDBPatch.cs**: Harmony 补丁
  - 游戏加载时初始化 ChromaDB
  - 游戏退出时关闭数据库连接

## 工作流程

### 1. 游戏加载

```
Game.LoadGame() 
  ↓ (Harmony Hook)
  ↓ ChromaDBPatch.GameLoadPatch.Postfix()
  ↓ ChromaService.InitializeForSave(saveId)
  ↓ ChromaClient 启动 Python 进程 (ChromaManager.py)
  ↓ 为当前存档创建/打开 ChromaDB collection
```

### 2. 生成对话

```
TalkService.GenerateTalk()
  ↓ 
  ↓ GenerateAndProcessTalkAsync()
  ├─ 查询历史上下文 (ChromaService.QueryRelevantContextAsync)
  │   ↓ ChromaClient 发送 query_context 命令
  │   ↓ Python 执行向量搜索 (BGE-M3 embedding)
  │   ↓ 返回 5 条最相关的历史对话
  │
  ├─ 用历史信息增强 prompt
  │
  ├─ 生成新对话 (AIService.ChatStreaming)
  │
  └─ 存储对话到数据库 (ChromaService.StoreConversationAsync)
      ↓ 异步线程
      ↓ ChromaClient 发送 add_conversation 命令
      ↓ Python 添加元数据记录
```

### 3. 数据结构

#### 存储的对话

```json
{
  "text": "对话内容文本",
  "speaker": "说话人名字",
  "listeners": ["听众1", "听众2"],
  "date": "In-game 日期",
  "talk_type": "Normal|Urgent|User|Event"
}
```

#### 查询结果

```csharp
public class ContextEntry
{
    public string Text { get; set; }           // 对话内容
    public string Speaker { get; set; }        // 说话人
    public List<string> Listeners { get; set; } // 听众
    public string Date { get; set; }           // 时间
    public string TalkType { get; set; }       // 对话类型
    public float Relevance { get; set; }       // 相关性分数 (低=更相关)
}
```

## 配置

### 数据库位置

- **路径**: `./chromadb/<save_id>/`
- **集合名**: `conversations`
- **条目上限**: 800,000（达到上限时自动清理最旧的 10%）

### 查询参数

- **max_results**: 5（可在 TalkService 中调整）
- **相关性过滤**: 自动按照说话人筛选历史

## 故障排除

### Python 进程启动失败

- 检查 `D:\Program\.venv\Scripts\python.exe` 是否可访问
- 确保安装了必要包: `chromadb`, `FlagEmbedding`
- 检查 ChromaManager.py 路径是否正确

### 查询超时

- 增加 ChromaClient 中的超时时间（当前 30 秒）
- 检查是否有大量同时查询操作

### 数据库损坏

- 删除 `./chromadb/<save_id>/` 目录重置
- 下一次加载时将创建新的空数据库

## 性能

- **存储**: 异步，不阻塞游戏线程
- **查询**: 异步等待，在生成对话前完成
- **向量化**: 使用 BGE-M3 (15M 参数)，GPU 加速时约 10-50ms/查询

## 安全性

- 每个存档完全隔离的数据库
- 元数据不包含敏感信息
- 自动清理过期数据

## 扩展

### 添加自定义元数据

在 `ChromaManager.add_conversation()` 中修改 metadata dict：

```python
metadata = {
    "save_id": save_id,
    "speaker": response.get("name"),
    "listeners": json.dumps(listeners),
    "date": date_string,
    "talk_type": response.get("talk_type"),
    # 添加自定义字段:
    "location": location_string,
    "mood": mood_value,
}
```

### 调整查询策略

在 `ChromaManager.query_relevant_context()` 中修改 where_filter 逻辑以支持更复杂的筛选。

## 文件清单

- `Source/ChromaManager/ChromaManager.py` - Python 核心模块
- `Source/ChromaManager/main.py` - 初始化脚本
- `Source/Service/ChromaService.cs` - C# 高级接口
- `Source/Service/ChromaClient.cs` - C# IPC 通信
- `Source/Patch/ChromaDBPatch.cs` - 游戏钩子
- `Source/Service/TalkService.cs` - 集成对话生成


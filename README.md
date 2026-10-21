# Proxifier-Keygen-CSharp

原项目：<https://github.com/y9nhjy/Proxifier-Keygen>

Proxifier 4.x 注册码生成器。保留原始 Python 实现，新增 C# 移植版与 WinForms 图形界面。

## 声明 / Disclaimer

本项目仅供学习与逆向工程研究，与 Initex Software（Proxifier 开发商）无任何关联，未获其授权。
项目仅包含算法分析与独立实现，不包含、不传播 Proxifier 软件本体或破解补丁。
原始算法实现来自 [y9nhjy/Proxifier-Keygen](https://github.com/y9nhjy/Proxifier-Keygen)。
若有侵权请联系删除。

This project is for educational and reverse engineering research only. It is not affiliated with or
authorized by Initex Software. No Proxifier binaries or cracks are included or distributed.
Original implementation: <https://github.com/y9nhjy/Proxifier-Keygen>. Contact us for removal requests.

## 目录

| 路径 | 说明 |
|------|------|
| `Pro_Keygen.py` | 原始 Python 版生成器 |
| `Pro_Checker.py` | 校验算法（逆向还原，用于验证 key） |
| `csharp/ui/` | C# 移植版 + WinForms 图形界面 |
| `csharp/ui/README.md` | C# 版详细说明（编译、算法、验证） |

## 快速开始

### Windows — 图形界面

双击 `csharp/ui/Pro_KeygenUI.exe`，选版本，点「生成 Key」。

### Windows — 命令行

```bash
csharp/ui/Pro_Keygen.exe                  # setup 安装版
csharp/ui/Pro_Keygen.exe -v portable      # portable 便携版
csharp/ui/Pro_Keygen.exe -v mac           # mac 版
```

### Python

```bash
python3 Pro_Keygen.py                     # setup
python3 Pro_Keygen.py -v portable
python3 Pro_Keygen.py -v mac
```

## 版本与产品位

key 内的 product 位必须与目标安装匹配，否则 Proxifier 会拒绝：

| 值 | 版本 |
|----|------|
| 0 | Setup 安装版 |
| 1 | Portable 便携版 |
| 2 | Mac 版 |

## 运行要求

- C# 版：.NET Framework 4.x（Windows 10 / 11 自带）
- Python 版：Python 3

C# 版算法与 Proxifier 4.07 的校验函数逐条对齐，详见 [`csharp/ui/README.md`](csharp/ui/README.md)。

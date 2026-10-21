# Proxifier Keygen — C# / WinForms

原项目：<https://github.com/y9nhjy/Proxifier-Keygen>

Proxifier 4.x 注册码生成器。算法从 `Pro_Keygen.py` 1:1 移植到 C#，附带 WinForms 图形界面。

## 目录

| 文件 | 说明 |
|------|------|
| `Keygen.cs` | 算法核心，CLI 与 GUI 共用 |
| `Pro_Keygen.cs` | 命令行入口 |
| `Pro_KeygenUI.cs` | WinForms 图形界面入口 |
| `Pro_Keygen.exe` | 命令行版（5.6 KB） |
| `Pro_KeygenUI.exe` | 图形界面版（7.2 KB） |

## 运行要求

.NET Framework 4.x。Windows 10 / 11 系统自带，无需安装运行时，拷贝即用。

## 使用

### 图形界面

双击 `Pro_KeygenUI.exe`：

1. 选择版本（Setup 安装版 / Portable 便携版 / Mac 版）
2. 点「生成 Key」或按回车
3. 结果自动全选，`Ctrl+C` 复制

### 命令行

```bash
Pro_Keygen.exe                  # setup 版（默认）
Pro_Keygen.exe -v portable      # portable 版
Pro_Keygen.exe -v mac           # mac 版
Pro_Keygen.exe --version=mac    # 等号形式
```

参数错误时输出 `版本参数错误!` 并返回非零退出码。

## 编译

用 .NET Framework 自带的 `csc.exe`（C# 5 语法）：

```bat
:: 命令行版
csc -nologo -codepage:65001 -optimize+ -target:exe -out:Pro_Keygen.exe ^
    Keygen.cs Pro_Keygen.cs

:: 图形界面版
csc -nologo -codepage:65001 -optimize+ -target:winexe -out:Pro_KeygenUI.exe ^
    -r:System.Windows.Forms.dll -r:System.Drawing.dll ^
    Keygen.cs Pro_KeygenUI.cs
```

Git Bash 下必须用 `-` 前缀，`/nologo` 会被 MSYS 转成路径导致 CS2001。

## 产品位

key 内部 `low_4B` 的 bit 21-22 标记产品版本，必须与目标安装匹配：

| 值 | 版本 |
|----|------|
| 0 | Setup 安装版 |
| 1 | Portable 便携版 |
| 2 | Mac 版 |

用错版本会提示 `This registration key is for another version of this product.`

## 算法

key 为 25 字符，用自定义 base32 字符表编码，分 5 组输出：

```
XXXXX-XXXXX-XXXXX-XXXXX-XXXXX
```

5 组各 5 字符，共 29 字符（含 4 个连字符）。

- 字符表：`0123456789ABCDEFGHIJKLMNOPQRSTUVWXZY`（末尾是 `X Z Y`）
- 去连字符后第 3 位是干扰位，校验时被丢弃，不影响 CRC
- `low_4B` / `mid_4B` / `high_4B` 经 XOR 还原后送入 CRC32（poly `0x04C11DB7`，无反射）
- CRC 低 25 位即最后一组字符
- `mid_4B` 高 16 位为 0 表示无限期，生成器固定写 0
- `low_4B` 低 16 位 < `0x2580` 视为过期

## 验证

- 校验算法与 Proxifier 4.07 静态反汇编逐条对齐（校验函数 RVA `0x2f80–0x3042`，`handle` 在 `0x30e0`，CRC 在 `0x31a0`）
- key 长度检查 `cmp qword [rdx+0x10], 0x1d` = 29 字符
- 300 个生成 key（三版本各 100）通过独立 Python 校验器：CRC 0 失败、product 位全对、无过期、无重复
- 移植差异：Python 的大整数 `(high<<64)+(mid<<32)+low` 在 C# 用 12 字节小端数组代替；`res *= 2` 的溢出判断改为检查 bit31 进位。数学等价
- 随机源从 Python MT 换成 `System.Random`，key 序列不同，生成规则一致

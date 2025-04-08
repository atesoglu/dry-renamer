# 📂 FileRenamer

**FileRenamer** is a simple command-line tool written in Go to batch rename files in a directory by cleaning and formatting their names. It supports dry-run mode to preview changes before applying them.

---

## ✨ Features

- Removes unwanted patterns (e.g., `sanet.st`)
- Converts file names to Title Case
- Replaces multiple underscores with spaces
- Supports a dry-run mode with multiple shorthand flags
- Cross-platform compatible (Linux, macOS, Windows)

---

## 🛠️ Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/FileRenamer.git
   cd FileRenamer
   ```

2. Build the program:
   ```bash
   go build -o filerenamer
   ```

---

## 🚀 Usage

```bash
./filerenamer <folderPath> [--dry-run | -d | -preview]
```

### Examples

✅ Rename files in the folder:

```bash
./filerenamer ./Downloads
```

🔍 Preview renaming without making changes:

```bash
./filerenamer ./Downloads --dry-run
./filerenamer ./Downloads -d
./filerenamer ./Downloads -preview
```

---

## 🧼 What Does It Clean?

- Removes patterns like `sanet.st`, `sanet_st`, etc.
- Replaces multiple underscores (`___`) with spaces
- Trims whitespace
- Converts to **Title Case** (e.g., `my_file_name.pdf` → `My File Name.pdf`)

---

## 📄 License

MIT License — see [`LICENSE`](./LICENSE) for details.

---

## 🙌 Contributions

Contributions, issues, and feature requests are welcome! Feel free to fork the repo and submit a pull request.

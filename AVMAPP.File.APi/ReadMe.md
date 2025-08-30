# 📂 Dosya Yönetim API'si

Bu proje, dosya yükleme, indirme, listeleme ve silme işlemleri için geliştirilmiş bir ASP.NET Core Web API uygulamasıdır.

## 📌 Kullanılan Teknolojiler
- .NET 8
- ASP.NET Core Web API
- Swagger UI
- JSON Format
- RESTful mimari
## 📌 API Kullanım Şeması
```
                          ┌───────────────────────────┐
                          │       İstemci (Client)    │
                          └───────────┬──────────────┘
                                      │
            ┌─────────────────────────────────────────────────────┐
            │                      API Server                     │
            │                                                     │
            │  1️⃣ Dosya Yükleme                                   │
            │  ─────────────────────────────────────────────────  │
            │  [POST] /api/files/upload                          │
            │  Body: { file: "test.pdf" }                        │
            │  → Sunucuya yeni bir dosya yükler                  │
            │                                                     │
            │  2️⃣ Dosya Listeleme                                │
            │  ─────────────────────────────────────────────────  │
            │  [GET] /api/files/list                             │
            │  Yanıt: [ "test.pdf", "image.png" ]                │
            │  → Mevcut tüm dosyaları listeler                   │
            │                                                     │
            │  3️⃣ Dosya İndirme                                  │
            │  ─────────────────────────────────────────────────  │
            │  [GET] /api/files/download/{fileName}              │
            │  Örnek: /api/files/download/test.pdf               │
            │  → test.pdf dosyasını istemciye gönderir           │
            │                                                     │
            │  4️⃣ Dosya Silme                                   │
            │  ─────────────────────────────────────────────────  │
            │  [DELETE] /api/files/delete/{fileName}             │
            │  Örnek: /api/files/delete/test.pdf                 │
            │  → test.pdf dosyasını sunucudan siler              │
            │                                                     │
            └─────────────────────────────────────────────────────┘
                                      │
                          ┌───────────▼───────────┐
                          │     Dosya Depolama    │
                          │   (Uploads Klasörü)   │
                          └───────────────────────┘
```

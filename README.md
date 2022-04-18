# Ưu điểm
- Code thoáng, dễ nhìn.
- Có comment thường xuyên, mô tả khá chi tiết.
- Các folder trong assets phân chia cẩn thận, rõ ràng.
- Có dùng Singleton trong Gamemanager.

# Nhược điểm
- Mỗi object lại có các trường SerializeField nên rất khó kiểm soát.
- Không dùng namespace.
- Nhiều chỗ còn dùng GetComponent.
- Nên dùng Pool thay vì Instantiate object rồi lại Destroy.
- Nên chia texture atlas ra thành các atlas nhỏ hơn, ví dụ những ảnh liên quan tới gameplay cho vào 1 atlas, những ảnh liên quan tới UI thì cho vào 1 atlas.
- Chưa nén ảnh và âm thanh.

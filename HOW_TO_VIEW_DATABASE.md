# 📊 UniPlanner 데이터베이스 보는 방법

## 데이터베이스 위치
```
G:\UTS\2025_Spring\DotNET\UniPlanner\bin\Debug\Data\uni.db
```

---

## 방법 1: DB Browser for SQLite (추천) ⭐

### 설치
1. https://sqlitebrowser.org/dl/ 접속
2. "DB Browser for SQLite" 다운로드 및 설치

### 사용법
1. **DB Browser for SQLite** 실행
2. **"Open Database"** 클릭
3. `bin\Debug\Data\uni.db` 파일 선택
4. **"Browse Data"** 탭에서 테이블 데이터 확인
   - `Subjects`: 과목 정보
   - `Schedule`: 시간표 정보
   - `Tasks`: 과제 정보
   - `Todos`: 개인 할일 정보

---

## 방법 2: Visual Studio Code + SQLite Viewer 확장

### 설치
1. VS Code 실행
2. 확장(Extensions) 메뉴에서 "SQLite Viewer" 검색 및 설치

### 사용법
1. VS Code에서 `bin\Debug\Data\uni.db` 파일 열기
2. 우클릭 → "Open Database" 선택
3. 좌측 SQLITE EXPLORER에서 테이블 확인

---

## 방법 3: PowerShell로 직접 조회

### 명령어
```powershell
# 프로젝트 디렉터리로 이동
cd G:\UTS\2025_Spring\DotNET\UniPlanner

# SQLite 명령어 실행 (SQLite3가 설치되어 있는 경우)
sqlite3 bin\Debug\Data\uni.db "SELECT * FROM Subjects;"
sqlite3 bin\Debug\Data\uni.db "SELECT * FROM Schedule;"
sqlite3 bin\Debug\Data\uni.db "SELECT * FROM Tasks;"
sqlite3 bin\Debug\Data\uni.db "SELECT * FROM Todos;"
```

---

## 방법 4: 프로그램 내에서 DB 초기화

### 데이터베이스 삭제 후 재생성
```powershell
# DB 삭제
Remove-Item -Path "bin\Debug\Data\uni.db" -Force

# 프로그램 실행하면 자동으로 샘플 데이터가 포함된 새 DB 생성됨
```

---

## 📋 테이블 구조

### Subjects (과목)
| 컬럼 | 타입 | 설명 |
|------|------|------|
| Id | INTEGER | 자동 증가 ID |
| Code | TEXT | 과목 코드 (예: COMP101) |
| Name | TEXT | 과목 이름 |
| Instructor | TEXT | 강사 이름 |
| Credits | INTEGER | 학점 |
| Color | TEXT | UI 색상 (Hex) |

### Schedule (시간표)
| 컬럼 | 타입 | 설명 |
|------|------|------|
| Id | INTEGER | 자동 증가 ID |
| DayOfWeek | INTEGER | 요일 (0=일, 1=월, ...) |
| Subject | TEXT | 과목 코드 |
| StartTime | TEXT | 시작 시간 |
| EndTime | TEXT | 종료 시간 |
| Location | TEXT | 강의실 위치 |
| Instructor | TEXT | 강사 이름 |

### Tasks (과제)
| 컬럼 | 타입 | 설명 |
|------|------|------|
| Id | INTEGER | 자동 증가 ID |
| Title | TEXT | 과제 제목 |
| DueDate | TEXT | 마감일 |
| Priority | TEXT | 우선순위 (High/Medium/Low) |
| IsCompleted | INTEGER | 완료 여부 (0/1) |
| Subject | TEXT | 과목 코드 |
| Description | TEXT | 설명 |

### Todos (개인 할일)
| 컬럼 | 타입 | 설명 |
|------|------|------|
| Id | INTEGER | 자동 증가 ID |
| Title | TEXT | 할일 제목 |
| IsCompleted | INTEGER | 완료 여부 (0/1) |
| Category | TEXT | 카테고리 |
| CreatedDate | TEXT | 생성일 |

---

## 🔍 샘플 쿼리

### 모든 과목 보기
```sql
SELECT Code, Name, Instructor, Credits FROM Subjects;
```

### 월요일 시간표 보기
```sql
SELECT * FROM Schedule WHERE DayOfWeek = 1 ORDER BY StartTime;
```

### 완료되지 않은 과제 보기
```sql
SELECT Title, DueDate, Priority, Subject FROM Tasks WHERE IsCompleted = 0;
```

### 미완료 할일 보기
```sql
SELECT Title, Category FROM Todos WHERE IsCompleted = 0;
```


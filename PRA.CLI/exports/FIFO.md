# FIFO — Simulation Result

**Reference string:** 7 0 1 2 0 3 0 4 2 3 0 3 2  
**Frames:** 3  
**Hits:** 3  
**Faults:** 10  
**Hit ratio:** 23.1%

| Step | Page | F0 | F1 | F2 | Result | Replaced |
|---|---|---|---|---|---|---|
| 1 | 7 | 7 | - | - | Fault | - |
| 2 | 0 | 7 | 0 | - | Fault | - |
| 3 | 1 | 7 | 0 | 1 | Fault | - |
| 4 | 2 | 2 | 0 | 1 | Fault | 7 |
| 5 | 0 | 2 | 0 | 1 | Hit | - |
| 6 | 3 | 2 | 3 | 1 | Fault | 0 |
| 7 | 0 | 2 | 3 | 0 | Fault | 1 |
| 8 | 4 | 4 | 3 | 0 | Fault | 2 |
| 9 | 2 | 4 | 2 | 0 | Fault | 3 |
| 10 | 3 | 4 | 2 | 3 | Fault | 0 |
| 11 | 0 | 0 | 2 | 3 | Fault | 4 |
| 12 | 3 | 0 | 2 | 3 | Hit | - |
| 13 | 2 | 0 | 2 | 3 | Hit | - |

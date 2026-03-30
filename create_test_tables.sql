-- Create test_cases table
CREATE TABLE IF NOT EXISTS test_cases (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL,
    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE,
    INDEX idx_project_id (project_id)
);

-- Create test_steps table
CREATE TABLE IF NOT EXISTS test_steps (
    id INT AUTO_INCREMENT PRIMARY KEY,
    test_case_id INT NOT NULL,
    step_order INT NOT NULL,
    action VARCHAR(100) NOT NULL,
    selector VARCHAR(500),
    value TEXT,
    created_at DATETIME NOT NULL,
    FOREIGN KEY (test_case_id) REFERENCES test_cases(id) ON DELETE CASCADE,
    INDEX idx_test_case_id (test_case_id),
    INDEX idx_step_order (test_case_id, step_order)
);

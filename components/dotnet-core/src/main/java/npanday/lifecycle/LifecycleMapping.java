/* 
 * Copyright 2010 NPanday
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package npanday.lifecycle;

import java.util.ArrayList;
import java.util.List;

import npanday.ArtifactType;

/**
 * @author <a href="mailto:me@lcorneliussen.de">Lars Corneliussen</a>
 */
public class LifecycleMapping {
	ArtifactType type;

	List<LifecyclePhase> phases = new ArrayList<LifecyclePhase>();

	public ArtifactType getType() {
		return type;
	}

	public void setType(ArtifactType type) {
		this.type = type;
	}

	public List<LifecyclePhase> getPhases() {
		return phases;
	}

	public void setPhases(List<LifecyclePhase> phases) {
		this.phases = phases;
	}

}
